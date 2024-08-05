using Microsoft.AspNetCore.Mvc;

using CORE1.Models; //Added for easy access to UserDetailViewModel

namespace CORE1.Controllers
{
    //This is a new MVC controller. It will handle all logic 
    //related to our ViewBagData views. When we create a new
    //controller, we get a default Action for an Index view,
    //but we don't get the actual View or its folder until 
    //we generate it.
    public class ViewBagDataController : Controller
    {
        //In order to accept the form data from the View, we need to add parameters
        //to the Action. The parameter names MUST match the "name" attributes in the form
        //(spelling matters, casing does not).
        public IActionResult Index(string firstName = "", string lastName = "")
        {
            //Store a string in the ViewBag, which will be passed to the View
            ViewBag.test = "Hello!!!";

            ////Check if the user has submitted any information for firstName or lastName
            //if(!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            //{
                  ////If so, use the info they provided in a string that will be stored
            //    ViewBag.firstName = firstName;
            //    ViewBag.lastName = lastName;
            //}
            //else
            //{
            //    ViewBag.firstName = string.Empty; 
            //    ViewBag.lastName = string.Empty;
            //}

            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;

            //Any logic to be performed in the Action MUST be done BEFORE the return View();
            return View();

            //int x = 100;
            //This is unreachable code -- Uncomment the line above to see the warning.
        }

        //Before creating this Action and it's corresponding View, we created a 
        //UserDetailViewModel in the Models folder
        public IActionResult ViewModels() {
            //When the user navigates to this View, create a UserDetailViewModel object,
            //store it in the ViewBag, and display it in the View

            UserDetailViewModel user1 = new UserDetailViewModel(117, "John", "UNSC", new DateTime(2150, 01, 11), "Chief@gmail.com", "../img/master_chief.jpg");

            ViewBag.User = user1;

            return View();
        }

        public IActionResult WaterWeight(int? nbrGallons)
        {
            //The "int?" datatype above designates the nbrGallons parameter as a NULLABLE int
            //This is useful for the instance the user wants to visit the WaterWeight View,
            //where they have not yet had the opportunity to provide a value for 
            //the number of gallons they would like to convert

            //Check if the user has provided a value for nbrGallons
            if (nbrGallons == null)
            {
                //If they haven't, set ViewBag.WaterWeight to null
                ViewBag.WaterWeight = null;
            }
            else
            {
                //If they have, calculate the result, insert it into a string, and return
                //it to the View
                ViewBag.WaterWeight = $"{nbrGallons} gallons of water weighs approximately " +
                    8.33 * nbrGallons + "lbs.";
            }

            return View();
        }


        public IActionResult UserDetails()
        {
            //Create three unique UserDetailViewModel objects, each using one of the avatar_# images
            //in the wwwroot's img folder

            UserDetailViewModel udvm1 = new UserDetailViewModel(001, "Jane", "Doe",
                new DateTime(1987, 2, 14), "jdoe@gmail.com", "../img/avatar_1.png");

            UserDetailViewModel udvm2 = new UserDetailViewModel(002, "Lori", "Jensen",
                new DateTime(1981, 5, 7), "ljensen@aol.com", "../img/avatar_2.png");

            UserDetailViewModel udvm3 = new UserDetailViewModel(003, "Daniel", "Smith",
                new DateTime(1996, 6, 30), "dsmith@msn.com", "../img/avatar_3.png");

            //Create a List<UserDetailViewModel> to store the UserDetailViewModel objects
            List<UserDetailViewModel> users = new List<UserDetailViewModel>()
            { udvm1, udvm2, udvm3 };

            //Store the list of UserDetailViewModel objects in a ViewBag variable to be
            //passed to the View for display
            ViewBag.Users = users;

            return View();
        }
    }
}
