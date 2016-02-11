using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IO;
using SelectPdf;
using NeedleWork2016.Core;
using NeedleWork2016.Entities;
using Microsoft.AspNet.Razor;
using System.Drawing;

namespace NeedleWork2016.Controllers
{
    //This controller realize the right way to create pdf report
    // I use filter which consists in the Filter folder
    public class PdfGenerateController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Thіs method generates a file in jpg format, method is called from PDFreport.cshtml
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ActionResult GetPhoto(string s)
        {
            PDFdata p = new PDFdata();
            byte[] data1 = p.StringToCode(s);
            return File(data1, "image/jpeg");
        }
        /// <summary>
        /// This method get data from the frontend, save this data in the ViewBag and return view PDFreport.cshtml. The filter is applied to this method ConvertToPdfFilterAttribute
        /// </summary>
        /// <param name="imageData1"></param>
        /// <param name="imageData2"></param>
        /// <param name="imageData3"></param>
        /// <returns></returns>
        public ActionResult GenerateDataForPDF(string imageData1, string imageData2, string imageData3)
        {
            //ViewBag will consist parametrs from the frontend
            ViewBag.image1 = "iVBORw0KGgoAAAANSUhEUgAAABUAAAAWCAIAAACg4UBvAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAsSAAALEgHS3X78AAAAFnRFWHRDcmVhdGlvbiBUaW1lADA1LzA1LzA35V+tVgAAABh0RVh0U29mdHdhcmUAQWRvYmUgRmlyZXdvcmtzT7MfTgAAA1BJREFUOI1tVE9o21Yc/iRLTqzYTkOyLYTqMBqsQ50l4FJo11K2QteymTkM6kAOG0sxHf0zNqjdw07byUxh0JJDGGyBUUh8KaSwW0kYa06BpetgdprDqBMTmjgk0pNk/X07KFZcOx9I6Pfxvt/ve4+nj6GUogOF8ZFOsvj4RSfJtOpbZSE2fu7jd0Xpvfrm6srvW0RRjm9Em8hnkpqiBOWrcvn762e12hV56nIrrylKPpMMSjaY/OV3c0IsBkBX1WqlIkqScGJs+huSOHtdiMWqlYquqgCEWOzCtTtHToPhQftcKpVLpRZnZ1+Vy7lUSlOUeVnOpVJfX7rkGymvrgbr2WAjfnv/DaBaqfgfQixmqCoAg5B6reaTrs75Fjjf/IUPe9eWl8+n0wNDQ1/Jcr1WO5dOl6anRy8OrTx58kkuJyYSJyVJlCQAa8vLF6+yK380/eczSa12TZ766K+lpeBgFmdn5RtXqVGUb3wa8JqiPFtclKc+0LbO7P97Op9JMvlM8v6PfX2D/WB6Fx7W15/zQvwtXdlJjHVl7ybg/Qdg4cH6+t8DQnyovlUZfZ9kbwEAeW39cI9yAIQoC7AAm717CkwvmB5QEd5L2EuADiB7C8AeGB2UtN0fDgCo/3iAC7jw9uCugu6/udIENVtr/+KxAFzbAXUWHm49LW3CI3CedYiPMFdEdQMAPKeptw0DMLO345Go+7T0HDCOVeoECzO4/BnEYTgNzzG9pl43LI3AI+ev7Jw8pc4VsbvdLl77E6UZpD+HOAwApuZYune4//V/nNORA767wbCMNAZxGKUZAOgfPBRXNyCN4ovCYWlprqk4JrEBnqGUFsZHvr3vRN/me/rDwcDdbdS3AUAchhA9MuLaHnltkR37wU/h4uMXnM9ublgiC+pC6OfYEAtgYBADg2iDbbj6nq3V7d2aBYQR/P+F8ZGbOT3Sx0d6ue44xwus3yWAY7kW8RoHtnHgGvvWz79G/RQ4yo/C+MjkBOl7h+/qCfERlutiQzwLgFLq2tQxXMvwTNVuqO4vv8WCCGnPn8kJIkTAdbOhcCjEMQCoB8+lTsO1DNe0mUfz0db8eUOPZoRNThBb8/xjoxQ6oeFo6NF8FB0p2K5v7dKGY/Pzf4k47RuDcix4AAAAAElFTkSuQmCC";
            ViewBag.image2 = "R0lGODdhMAAwAPAAAAAAAP///ywAAAAAMAAwAAAC8IyPqcvt3wCcDkiLc7C0qwyGHhSWpjQu5yqmCYsapyuvUUlvONmOZtfzgFzByTB10QgxOR0TqBQejhRNzOfkVJ + 5YiUqrXF5Y5lKh / DeuNcP5yLWGsEbtLiOSpa / TPg7JpJHxyendzWTBfX0cxOnKPjgBzi4diinWGdkF8kjdfnycQZXZeYGejmJlZeGl9i2icVqaNVailT6F5iJ90m6mvuTS4OK05M0vDk0Q4XUtwvKOzrcd3iq9uisF81M1OIcR7lEewwcLp7tuNNkM3uNna3F2JQFo97Vriy / Xl4 / f1cf5VWzXyym7PHhhx4dbgYKAAA7";
            ViewBag.image3 = "iVBORw0KGgoAAAANSUhEUgAAABUAAAAWCAIAAACg4UBvAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAsSAAALEgHS3X78AAAAFnRFWHRDcmVhdGlvbiBUaW1lADA1LzA1LzA35V+tVgAAABh0RVh0U29mdHdhcmUAQWRvYmUgRmlyZXdvcmtzT7MfTgAAA1BJREFUOI1tVE9o21Yc/iRLTqzYTkOyLYTqMBqsQ50l4FJo11K2QteymTkM6kAOG0sxHf0zNqjdw07byUxh0JJDGGyBUUh8KaSwW0kYa06BpetgdprDqBMTmjgk0pNk/X07KFZcOx9I6Pfxvt/ve4+nj6GUogOF8ZFOsvj4RSfJtOpbZSE2fu7jd0Xpvfrm6srvW0RRjm9Em8hnkpqiBOWrcvn762e12hV56nIrrylKPpMMSjaY/OV3c0IsBkBX1WqlIkqScGJs+huSOHtdiMWqlYquqgCEWOzCtTtHToPhQftcKpVLpRZnZ1+Vy7lUSlOUeVnOpVJfX7rkGymvrgbr2WAjfnv/DaBaqfgfQixmqCoAg5B6reaTrs75Fjjf/IUPe9eWl8+n0wNDQ1/Jcr1WO5dOl6anRy8OrTx58kkuJyYSJyVJlCQAa8vLF6+yK380/eczSa12TZ766K+lpeBgFmdn5RtXqVGUb3wa8JqiPFtclKc+0LbO7P97Op9JMvlM8v6PfX2D/WB6Fx7W15/zQvwtXdlJjHVl7ybg/Qdg4cH6+t8DQnyovlUZfZ9kbwEAeW39cI9yAIQoC7AAm717CkwvmB5QEd5L2EuADiB7C8AeGB2UtN0fDgCo/3iAC7jw9uCugu6/udIENVtr/+KxAFzbAXUWHm49LW3CI3CedYiPMFdEdQMAPKeptw0DMLO345Go+7T0HDCOVeoECzO4/BnEYTgNzzG9pl43LI3AI+ev7Jw8pc4VsbvdLl77E6UZpD+HOAwApuZYune4//V/nNORA767wbCMNAZxGKUZAOgfPBRXNyCN4ovCYWlprqk4JrEBnqGUFsZHvr3vRN/me/rDwcDdbdS3AUAchhA9MuLaHnltkR37wU/h4uMXnM9ublgiC+pC6OfYEAtgYBADg2iDbbj6nq3V7d2aBYQR/P+F8ZGbOT3Sx0d6ue44xwus3yWAY7kW8RoHtnHgGvvWz79G/RQ4yo/C+MjkBOl7h+/qCfERlutiQzwLgFLq2tQxXMvwTNVuqO4vv8WCCGnPn8kJIkTAdbOhcCjEMQCoB8+lTsO1DNe0mUfz0db8eUOPZoRNThBb8/xjoxQ6oeFo6NF8FB0p2K5v7dKGY/Pzf4k47RuDcix4AAAAAElFTkSuQmCC";
            return View("~/Templates/Reports/PDFreport.cshtml");
        }
       
    }
}
