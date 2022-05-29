using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;
using Microsoft.EntityFrameworkCore.Query;

namespace UnitTestsProject;

public class UnitTestsClass
{
    private readonly Mock<IShopProfileRepository> _mockShopProfile;
    private readonly Mock<IAccountRepository> _mockUser;
    private readonly ShopProfileController _controller;

    public UnitTestsClass()
    {
        _mockShopProfile = new Mock<IShopProfileRepository>();
        _mockUser = new Mock<IAccountRepository>();
        
        _controller = new ShopProfileController(_mockShopProfile.Object, _mockUser.Object);
    }

    [Fact]
    public void IndexShopProfilesTest()
    {
        // Arrange
        _mockShopProfile.Setup(repo => repo.GetShopProfiles(null))
            .ReturnsAsync(new List<ShopProfile> { new ShopProfile(), new ShopProfile() });
        
        // Act
        var result = _controller.Index(null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result.Result);
        var shopProfiles = Assert.IsType<List<ShopProfile>>(viewResult.Model);
        Assert.Equal(2, shopProfiles.Count);
    }
    
    [Fact]
    public async void DetailsOfShopProfileTest()
    {
        // Arrange
        var shop = GetTestShopProfile();
        
        _mockShopProfile.Setup(repo => repo.GetShopProfileDetailedById(shop.Id))
            .ReturnsAsync(shop);
 
        // _mockOrder.Setup(repo => repo.GetShopOrders(shop.Id))
        //     .Returns();
        
        // Act
        var result = await _controller.Details(shop.Id);
 
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ShopProfile>(viewResult.ViewData.Model);
        Assert.Equal(shop.Name, model.Name);
        Assert.Equal(shop.Id, model.Id);
    }
    
    [Fact]
    public async void CreateShopProfileTest()
    {
        // Arrange
        var user = GetTestUser();
        var shop = GetTestShopProfileModel();

        _mockUser.Setup(repo => repo.GetCurrentUser(user.Email))
            .Returns(user);
        
        _mockShopProfile.Setup(repo => repo.CreateShopProfile(GetTestShopProfileModel(),user.Id))
            .ReturnsAsync(true).Verifiable();

        // Act
        var result = await _controller.Create(shop);

        //Assert
        _mockUser.Verify(r => r.GetCurrentUser(user.Email));
        _mockShopProfile.Verify(r => r.CreateShopProfile(shop,user.Id));
        Assert.NotNull(result);
        
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
    
    [Fact]
    public async void EditShopProfileTest()
    {
        // Arrange
        var shop = GetTestShopProfile();
        var model = GetTestShopProfileModel();
        
        _mockShopProfile.Setup(repo => repo.UpdateShopProfile(model.Id, model))
            .ReturnsAsync(shop).Verifiable();

        // Act
        var result = await _controller.Edit(model.Id, model);

        //Assert
        _mockShopProfile.Verify(r => r.UpdateShopProfile(model.Id, model));
        Assert.NotNull(result);
        
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
    
    [Fact]
    public async void DeleteConfirmedShopProfileTest()
    {
        //Arrange
        _mockShopProfile.Setup(repo => repo.DeleteShopProfile(1))
            .ReturnsAsync(true);
        
        //Act
        var result = await _controller.DeleteConfirmed(1);

        //Assert
        _mockShopProfile.Verify(r => r.DeleteShopProfile(1));
        Assert.NotNull(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
    
    private ShopProfile GetTestShopProfile()
    {
        var shopProfile = new ShopProfile
        {
            Id = 1,
            Name = "Test Shop Profile"
        };

        return shopProfile;
    }
    
    private ShopProfileViewModel GetTestShopProfileModel()
    {
        var shopProfile = new ShopProfileViewModel
        {
            Id = 1,
            Name = "Test Shop Profile",
            Username = "testuser@gmail.com"
        };

        return shopProfile;
    }

    private User GetTestUser()
    {
        var testUser = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "testuser@gmail.com"
        };

        return testUser;
    }
}