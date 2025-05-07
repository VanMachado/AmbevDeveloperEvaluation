using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISalesRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IDiscountService _discountService;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISalesRepository>();
        _mapper = Substitute.For<IMapper>();
        _discountService = Substitute.For<IDiscountService>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _discountService);
    }

    [Fact(DisplayName = "Given valid sale command When creating sale Then returns created sale result")]
    public async Task Handle_ValidRequest_ReturnsCreatedSale()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCommand();
        var sale = new Sale { Id = Guid.NewGuid(), SaleNumber = command.SaleNumber };
        var result = new CreateSaleResult { Id = sale.Id };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _discountService.CalculateDiscount(Arg.Any<int>(), Arg.Any<decimal>(), Arg.Any<SaleItem>()).Returns(10);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid sale command When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand(); // invalid by default

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given existing sale number When handling Then throws invalid operation exception")]
    public async Task Handle_ExistingSaleNumber_ThrowsException()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCommand();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(new Sale());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with SaleNumber {command.SaleNumber} and {command.Items.Count} items already exists");
    }

    [Fact(DisplayName = "Given valid items When calculating Then applies discount and validates quantity")]
    public async Task Handle_ValidItems_ValidatesDiscountsAndQuantities()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCommand();
        var sale = new Sale { Id = Guid.NewGuid() };

        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>()).Returns((Sale?)null);
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });

        _discountService.CalculateDiscount(Arg.Any<int>(), Arg.Any<decimal>(), Arg.Any<SaleItem>()).Returns(0.05m);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        foreach (var item in command.Items)
        {
            _discountService.Received(1).ValidateQuantityRules(item.Quantity);
            _discountService.Received(1).CalculateDiscount(item.Quantity, item.UnitPrice, item);
        }

        response.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given exception during execution When handling Then throws domain exception")]
    public async Task Handle_InternalError_ThrowsDomainException()
    {
        // Arrange
        var command = CreateSaleTestData.GenerateValidCommand();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        _discountService.When(d => d.ValidateQuantityRules(Arg.Any<int>()))
            .Do(_ => throw new Exception("Unexpected failure"));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Sale error while creating Sale number {command.SaleNumber}");
    }
}
