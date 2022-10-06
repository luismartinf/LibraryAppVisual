using LibraryAppAccess.AppData;
using LibraryAppAccess.Controllers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAppAccess.LibraryModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Shouldly;

namespace LibraryTest
{
    [TestClass]
    public class LeendeControllerTest
    {
        private ILibraryAppContext _context = new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");

        [TestMethod]
        public void GetLendees_LendeesToList_returns_numberofobjects()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            LendeeModelController fakecontroller = A.Fake<LendeeModelController>(x => x.WithArgumentsForConstructor(() => new LendeeModelController(mockRepository)));

            var dummies = (List<LendeeModel>)A.CollectionOfDummy<LendeeModel>(elemntsindb()-1);
            A.CallTo(() => fakecontroller.GetLendees()).Returns(dummies);

            // Act
            var controller = new LendeeModelController(null);
            var actionResult = controller.GetLendees();
            var content = (IList<LendeeModel>)actionResult.Value;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<IEnumerable<LendeeModel>>));
            Assert.AreEqual(dummies.Count, content.Count);
        }

        [TestMethod]
        public void GetLendeeModel_ReturnsLendeeWithSameId()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Find(2)).Returns(new LendeeModel { LendeeId = 2, Name = "Raul Garza", ContactNum = 0 });

            // Act
            var mockcontroller = new LendeeModelController(mockRepository);
            var controller = new LendeeModelController(null);
            var fakeLendee = mockcontroller.GetLendeeModel(2).Value; ;
            var contentResult = controller.GetLendeeModel(2).Value;


            // Assert
            A.CallTo(() => mockRepository.Lendees.Find(2)).MustHaveHappened();
            Assert.AreEqual(fakeLendee.LendeeId, contentResult.LendeeId);
            Assert.AreEqual(fakeLendee.Name, contentResult.Name);
        }

        [TestMethod]
        public void GetLendeeModel_ReturnsNotFound()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Find(elemntsindb() + 5)).Returns(null);

            // Act
            var controller = new LendeeModelController(mockRepository);
            var actionResult = controller.GetLendeeModel(elemntsindb() + 5).Result;


            // Assert
            A.CallTo(() => mockRepository.Lendees.Find(elemntsindb() + 5)).MustHaveHappened();
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));

        }


        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Find(2)).Returns(new LendeeModel { LendeeId = 2, Name = "Raul Garza", ContactNum = 0 });

            // Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.DeleteLendeeModel(2);

            // Assert
            A.CallTo(() => mockRepository.Lendees.Find(2)).MustHaveHappened();
            A.CallTo(() => mockRepository.Lendees.Remove(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            Assert.IsNotNull(actionResult);

        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Find(elemntsindb() + 5)).Returns(null);

            // Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.DeleteLendeeModel(elemntsindb() + 5);

            // Assert

            A.CallTo(() => mockRepository.Lendees.Find(elemntsindb() + 5)).MustHaveHappened();
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            A.CallTo(() => mockRepository.Lendees.Remove(A<LendeeModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustNotHaveHappened();
        }

        [TestMethod]
        public void DeleteReturnsBadrequest()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Find(3)).Returns(A.Fake<LendeeModel>()); ;
            A.CallTo(() => mockRepository.Lendees.Remove(A<LendeeModel>.Ignored)).Throws(new InvalidOperationException());

            // Act
            LendeeModelController controller = new(mockRepository);
            ActionResult<LendeeModel> actionResult;
            try
            {
                actionResult = controller.DeleteLendeeModel(3);
            }
            catch
            {
                actionResult = new BadRequestResult();
            }

            // Assert
            A.CallTo(() => mockRepository.Lendees.Find(3)).MustHaveHappened();
            A.CallTo(() => mockRepository.Lendees.Remove(A<LendeeModel>.Ignored)).MustHaveHappened();
            Assert.ThrowsException<InvalidOperationException>(() => mockRepository.Lendees.Remove(A<LendeeModel>.Ignored));
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
            A.CallTo(() => mockRepository.SaveChanges()).MustNotHaveHappened();
        }

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {

            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Add(A<LendeeModel>.Ignored)).Returns(new LendeeModel { LendeeId = elemntsindb(), Name = "Alejandra Cerezos", ContactNum = 0 });
            

            // Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.PostLendeeModel(A.Fake<LendeeModel>());

            // Assert
            A.CallTo(() => mockRepository.Lendees.Add(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<LendeeModel>));

        }


        [TestMethod]
        public void PostMethodReturnBadRequest()
        {

            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.Lendees.Add(A<LendeeModel>.Ignored)).Throws(new InvalidOperationException());

            // Act
            LendeeModelController controller = new(mockRepository);
            ActionResult<LendeeModel> actionResult;
            try
            {
                actionResult = controller.PostLendeeModel(A.Fake<LendeeModel>());
            }
            catch
            {
                actionResult = new BadRequestResult();
            }

            // Assert
            A.CallTo(() => mockRepository.Lendees.Add(A<LendeeModel>.Ignored)).MustHaveHappened();
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
            Assert.ThrowsException<InvalidOperationException>(() => mockRepository.Lendees.Add(A<LendeeModel>.Ignored));
            A.CallTo(() => mockRepository.SaveChanges()).MustNotHaveHappened();

        }

        [TestMethod]
        public void PutReturnsContentResult()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            LendeeModel mockLendee = A.Fake<LendeeModel>();
            mockLendee.LendeeId = 5;
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored));
            A.CallTo(() => mockRepository.SaveChanges());
            A.CallTo(() => mockLendee.LendeeModelExists(5)).Returns(true);
           
            //Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.PutLendeeModel(5, mockLendee);

            //assert
            Assert.IsInstanceOfType(actionResult, typeof(IActionResult));
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            A.CallTo(() => mockLendee.LendeeModelExists(5)).MustNotHaveHappened();
        }


        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            LendeeModel mockLendee = A.Fake<LendeeModel>();
         

            //Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.PutLendeeModel(1, mockLendee);

            //assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
            Assert.AreNotEqual(1, mockLendee.LendeeId);
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustNotHaveHappened();
        }

        [TestMethod]
        public void PutReturnsNotFound()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.SaveChanges()).Throws(new DbUpdateConcurrencyException());
            LendeeModel mockLendee = A.Fake<LendeeModel>();
            A.CallTo(() => mockLendee.LendeeModelExists(5)).Returns(false);
            mockLendee.LendeeId = 5;

            //Act
            LendeeModelController controller = new(mockRepository);
            var actionResult = controller.PutLendeeModel(5, mockLendee);

            //assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            Assert.ThrowsException<DbUpdateConcurrencyException>(() => mockRepository.SaveChanges());
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            A.CallTo(() => mockLendee.LendeeModelExists(5)).MustHaveHappened();
        }

        [TestMethod]
        public void PutReturnsFailMarkAsModified()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).Throws(new NotImplementedException());
            LendeeModel mockLendee = A.Fake<LendeeModel>();
            mockLendee.LendeeId = 5;
            //Act
            LendeeModelController controller = new(mockRepository);
            IActionResult actionResult;
            try
            {
                actionResult = controller.PutLendeeModel(5, mockLendee);
            }
            catch
            {
                actionResult = new NoContentResult();
            }

            //assert
            Assert.ThrowsException<NotImplementedException>(() => mockRepository.MarkAsModified(mockLendee));
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustNotHaveHappened();
            A.CallTo(() => mockLendee.LendeeModelExists(A<int>.Ignored)).MustNotHaveHappened();
        }   
        
        [TestMethod]
        public void PutReturnsNoContent()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.SaveChanges()).Throws(new DbEntityValidationException());
            LendeeModel mockLendee = A.Fake<LendeeModel>();
            mockLendee.LendeeId = 5;

            //Act
            LendeeModelController controller = new(mockRepository);
            IActionResult actionResult;
            try
            {
                actionResult = controller.PutLendeeModel(5, mockLendee);
            }
            catch
            {
                actionResult = new NoContentResult();
            }

            //assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            Assert.ThrowsException<DbEntityValidationException>(() => mockRepository.SaveChanges());
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            A.CallTo(() => mockLendee.LendeeModelExists(A<int>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        public void PutMethodNotcatchingConcurrencyException()
        {
            // Arrange
            ILibraryAppContext mockRepository = A.Fake<ILibraryAppContext>();
            A.CallTo(() => mockRepository.SaveChanges()).Throws(new DbUpdateConcurrencyException());
            LendeeModel mockLendee = A.Fake<LendeeModel>();
            mockLendee.LendeeId = 5;
            A.CallTo(() => mockLendee.LendeeModelExists(5)).Returns(true);

            // Act
            LendeeModelController controller = new(mockRepository);
            IActionResult actionResult;
            try
            {
                actionResult = controller.PutLendeeModel(5, mockLendee);
            }
            catch
            {
                actionResult = new JsonResult(new { Message = "Unknowerror" }) { StatusCode = 500};
            }

            // Assert
            A.CallTo(() => mockRepository.MarkAsModified(A<LendeeModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => mockRepository.SaveChanges()).MustHaveHappened();
            A.CallTo(() => mockLendee.LendeeModelExists(A<int>.Ignored)).MustHaveHappened();
            Assert.ThrowsException<DbUpdateConcurrencyException>(() => mockRepository.SaveChanges());
            actionResult.ShouldNotBeOfType(typeof(NotFoundResult));
        }

        private int elemntsindb()
        {
            var elementsdb = _context.Lendees.ToList().Count + 1;
            return elementsdb;
        }
    }
}
