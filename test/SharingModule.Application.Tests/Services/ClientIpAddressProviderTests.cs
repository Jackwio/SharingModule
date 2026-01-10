using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using SharingModule.Application.Services;
using Shouldly;
using Xunit;

namespace SharingModule.Services;

public class ClientIpAddressProviderTests
{
    [Fact]
    public void Should_Return_Null_When_HttpContext_Is_Null()
    {
        // Arrange
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns((HttpContext?)null);
        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBeNull();
    }

    [Fact]
    public void Should_Extract_IP_From_XForwardedFor_Header()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Forwarded-For"] = "203.0.113.1, 192.168.1.1, 10.0.0.1";

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBe("203.0.113.1");
    }

    [Fact]
    public void Should_Extract_Single_IP_From_XForwardedFor_Header()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Forwarded-For"] = "203.0.113.50";

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBe("203.0.113.50");
    }

    [Fact]
    public void Should_Trim_Whitespace_From_XForwardedFor_IP()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Forwarded-For"] = "  203.0.113.99  , 192.168.1.1";

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBe("203.0.113.99");
    }

    [Fact]
    public void Should_Fallback_To_RemoteIpAddress_When_No_XForwardedFor_Header()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("198.51.100.42");

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBe("198.51.100.42");
    }

    [Fact]
    public void Should_Return_Null_When_No_XForwardedFor_And_No_RemoteIpAddress()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = null;

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBeNull();
    }

    [Fact]
    public void Should_Return_Null_When_XForwardedFor_Is_Empty()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Forwarded-For"] = "";
        httpContext.Connection.RemoteIpAddress = null;

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBeNull();
    }

    [Fact]
    public void Should_Handle_IPv6_Address()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Forwarded-For"] = "2001:0db8:85a3:0000:0000:8a2e:0370:7334";

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var provider = new ClientIpAddressProvider(httpContextAccessor);

        // Act
        var ipAddress = provider.GetClientIpAddress();

        // Assert
        ipAddress.ShouldBe("2001:0db8:85a3:0000:0000:8a2e:0370:7334");
    }
}
