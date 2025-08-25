using UNIR.TFE.Polyrepo.Addition.Module.Application;

namespace UNIR.TFE.Polyrepo.Addition.Module.Test
{
    public class AdditionAppServiceTests
    {
        private readonly AdditionAppService _sut;

        public AdditionAppServiceTests()
        {
            _sut = new AdditionAppService();
        }

        [Fact]
        public void Key_ShouldReturn_Add_WhenRequested()
        {
            // Arrange
            const string expectedKey = "add";

            // Act
            var actualKey = _sut.Key;

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }

        // Pruebas básicas y casos extremos
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, 1, 0)]
        [InlineData(0, 0, 0)]
        [InlineData(123.45, 54.55, 178)]
        [InlineData(-5, -7, -12)]
        [InlineData(999999999, 1, 1000000000)]
        //[InlineData(decimal.MaxValue, 0, decimal.MaxValue)]
        //[InlineData(decimal.MinValue, 0, decimal.MinValue)]
        //[InlineData(0, decimal.MaxValue, decimal.MaxValue)]
        //[InlineData(0, decimal.MinValue, decimal.MinValue)]
        public void Execute_ShouldReturn_CorrectSum_ForGivenOperands(decimal operandA, decimal operandB, decimal expectedSum)
        {
            // Act
            var actualSum = _sut.Execute(operandA, operandB);

            // Assert
            Assert.Equal(expectedSum, actualSum);
        }

        [Fact]
        public void Execute_ShouldHandle_MaxDecimalValues()
        {
            // Arrange
            decimal maxValue = decimal.MaxValue;

            // Act & Assert
            var result = _sut.Execute(maxValue, 0);
            Assert.Equal(maxValue, result);
        }

        [Fact]
        public void Execute_ShouldHandle_MinDecimalValues()
        {
            // Arrange
            decimal minValue = decimal.MinValue;

            // Act & Assert
            var result = _sut.Execute(minValue, 0);
            Assert.Equal(minValue, result);
        }

        // Generación de pruebas con números secuenciales
        public static IEnumerable<object[]> SequentialNumbers()
        {
            for (int i = -500; i <= 500; i++)
            {
                yield return new object[] { i, i, i * 2 };
                yield return new object[] { i, -i, 0 };
                yield return new object[] { i, 0, i };
            }
        }

        [Theory]
        [MemberData(nameof(SequentialNumbers))]
        public void Execute_WithSequentialNumbers_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
        {
            // Act
            var result = _sut.Execute(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        // Pruebas con números decimales específicos
        public static IEnumerable<object[]> DecimalTestCases()
        {
            var testCases = new[]
            {
                (0.1m, 0.2m, 0.3m),
                (1.111m, 2.222m, 3.333m),
                (99.99m, 0.01m, 100.00m),
                (123.456m, 789.012m, 912.468m),
                (-45.67m, 45.67m, 0.00m),
                (1000.001m, 0.999m, 1001.000m),
                (0.0001m, 0.0001m, 0.0002m),
                (999.999m, 0.001m, 1000.000m)
            };

            foreach (var (a, b, expected) in testCases)
            {
                yield return new object[] { a, b, expected };
            }
        }

        [Theory]
        [MemberData(nameof(DecimalTestCases))]
        public void Execute_WithDecimalNumbers_ReturnsPreciseResult(decimal a, decimal b, decimal expected)
        {
            // Act
            var result = _sut.Execute(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        // Pruebas con números grandes
        public static IEnumerable<object[]> LargeNumbersTestCases()
        {
            var largeNumbers = new[]
            {
                1000000m,
                5000000m,
                10000000m,
                50000000m,
                100000000m,
                500000000m,
                1000000000m,
                5000000000m,
                10000000000m,
                50000000000m
            };

            foreach (var number in largeNumbers)
            {
                yield return new object[] { number, number, number * 2 };
                yield return new object[] { number, 0, number };
                yield return new object[] { -number, number, 0 };
            }
        }

        [Theory]
        [MemberData(nameof(LargeNumbersTestCases))]
        public void Execute_WithLargeNumbers_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
        {
            // Act
            var result = _sut.Execute(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        // Pruebas de propiedades matemáticas
        public static IEnumerable<object[]> CommutativePropertyTestCases()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 1000 - 500);
                decimal b = (decimal)(random.NextDouble() * 1000 - 500);
                yield return new object[] { a, b };
            }
        }

        [Theory]
        [MemberData(nameof(CommutativePropertyTestCases))]
        public void Execute_ShouldBeCommutative(decimal a, decimal b)
        {
            // Act
            var result1 = _sut.Execute(a, b);
            var result2 = _sut.Execute(b, a);

            // Assert
            Assert.Equal(result1, result2);
        }

        // Pruebas de asociatividad (a + b) + c = a + (b + c)
        public static IEnumerable<object[]> AssociativePropertyTestCases()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 1000 - 500);
                decimal b = (decimal)(random.NextDouble() * 1000 - 500);
                decimal c = (decimal)(random.NextDouble() * 1000 - 500);
                yield return new object[] { a, b, c };
            }
        }

        [Theory]
        [MemberData(nameof(AssociativePropertyTestCases))]
        public void Execute_ShouldBeAssociative(decimal a, decimal b, decimal c)
        {
            // Act
            var result1 = _sut.Execute(_sut.Execute(a, b), c);
            var result2 = _sut.Execute(a, _sut.Execute(b, c));

            // Assert
            Assert.Equal(result1, result2);
        }

        // Pruebas de elemento neutro (a + 0 = a)
        public static IEnumerable<object[]> IdentityElementTestCases()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 2000 - 1000);
                yield return new object[] { a };
            }
        }

        [Theory]
        [MemberData(nameof(IdentityElementTestCases))]
        public void Execute_WithZero_ShouldReturnSameNumber(decimal a)
        {
            // Act
            var result = _sut.Execute(a, 0);

            // Assert
            Assert.Equal(a, result);
        }

        // Pruebas de inverso aditivo (a + (-a) = 0)
        [Theory]
        [MemberData(nameof(IdentityElementTestCases))]
        public void Execute_WithInverse_ShouldReturnZero(decimal a)
        {
            // Act
            var result = _sut.Execute(a, -a);

            // Assert
            Assert.Equal(0, result);
        }

        // Pruebas de rendimiento con múltiples operaciones
        [Fact]
        public void Execute_ShouldHandle_MultipleOperationsCorrectly()
        {
            // Arrange
            decimal result = 0;
            decimal expected = 0;

            // Act
            for (int i = 1; i <= 1000; i++)
            {
                result = _sut.Execute(result, 1);
                expected += 1;
            }

            // Assert
            Assert.Equal(expected, result);
        }

        // Pruebas con números en los límites del decimal
        public static IEnumerable<object[]> EdgeCaseTestCases()
        {
            yield return new object[] { decimal.MaxValue, decimal.MaxValue };
            yield return new object[] { decimal.MinValue, decimal.MinValue };
            yield return new object[] { decimal.MaxValue, decimal.MinValue };
            yield return new object[] { decimal.MinValue, decimal.MaxValue };
            yield return new object[] { decimal.MaxValue, 1 };
            yield return new object[] { decimal.MinValue, -1 };
        }

        //[Theory]
        //[MemberData(nameof(EdgeCaseTestCases))]
        //public void Execute_WithEdgeCases_ShouldNotThrow(decimal a, decimal b)
        //{
        //    // Act & Assert (no debería lanzar excepción)
        //    var result = _sut.Execute(a, b);
        //    Assert.IsType<decimal>(result);
        //}

        // Pruebas de precisión decimal
        public static IEnumerable<object[]> PrecisionTestCases()
        {
            return new[]
            {
                new object[] { 0.0000000000000000000000000001m, 0.0000000000000000000000000001m, 0.0000000000000000000000000002m },
                new object[] { 0.0000000000000000000000000001m, 0.0000000000000000000000000002m, 0.0000000000000000000000000003m },
                new object[] { 1.2345678901234567890123456789m, 2.3456789012345678901234567890m, 3.5802467913580246791358024679m }
            };
        }

        [Theory]
        [MemberData(nameof(PrecisionTestCases))]
        public void Execute_WithHighPrecisionNumbers_MaintainsPrecision(decimal a, decimal b, decimal expected)
        {
            // Act
            var result = _sut.Execute(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}