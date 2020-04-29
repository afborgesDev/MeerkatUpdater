using FluentAssertions;
using MeerkatUpdater.Core.Runner.Command.Common;
using System;
using Xunit;

namespace MeerkatUpdater.Core.Test.TestsForServices.CommonServicesTests
{
    public class ArgumentValidationTest
    {
        [Fact]
        public void ShouldNotAllowInvalidEmptyArgumentsToExecuteACommand()
        {
            Action executeValidationForNull = () => ArgumentsValidation.Validate(null);
            Action executeValidationForEmpty = () => ArgumentsValidation.Validate();
            Action executeValidationForValid = () => ArgumentsValidation.Validate("");

            executeValidationForNull.Should().Throw<ArgumentNullException>();
            executeValidationForEmpty.Should().Throw<ArgumentNullException>();
            executeValidationForValid.Should().NotThrow();
        }
    }
}