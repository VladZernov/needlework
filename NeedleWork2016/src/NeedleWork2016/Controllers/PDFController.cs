using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.IO;
using Microsoft.AspNet.Hosting;
using iTextSharp.text;
using NeedleWork2016.Core;
using SelectPdf;

namespace NeedleWork2016.Controllers
{ //This controller realize another way to create pdf report
    public class PDFController : Controller
    {
 
        public ActionResult Index()
        {
            return View();
        }
   //
        public ActionResult DownloadReport(string imageData1, string imageData2, string imageData3)
        {
            byte[] pdfDocBytes;

            using (MemoryStream ms = new MemoryStream())

            using (Document doc = new Document())

            {

                PDF p = new PDF();

                p.Settings(ms, doc);

                byte[] data1 = p.StringToCode("iVBORw0KGgoAAAANSUhEUgAAABUAAAAWCAIAAACg4UBvAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAsSAAALEgHS3X78AAAAFnRFWHRDcmVhdGlvbiBUaW1lADA1LzA1LzA35V+tVgAAABh0RVh0U29mdHdhcmUAQWRvYmUgRmlyZXdvcmtzT7MfTgAAA1BJREFUOI1tVE9o21Yc/iRLTqzYTkOyLYTqMBqsQ50l4FJo11K2QteymTkM6kAOG0sxHf0zNqjdw07byUxh0JJDGGyBUUh8KaSwW0kYa06BpetgdprDqBMTmjgk0pNk/X07KFZcOx9I6Pfxvt/ve4+nj6GUogOF8ZFOsvj4RSfJtOpbZSE2fu7jd0Xpvfrm6srvW0RRjm9Em8hnkpqiBOWrcvn762e12hV56nIrrylKPpMMSjaY/OV3c0IsBkBX1WqlIkqScGJs+huSOHtdiMWqlYquqgCEWOzCtTtHToPhQftcKpVLpRZnZ1+Vy7lUSlOUeVnOpVJfX7rkGymvrgbr2WAjfnv/DaBaqfgfQixmqCoAg5B6reaTrs75Fjjf/IUPe9eWl8+n0wNDQ1/Jcr1WO5dOl6anRy8OrTx58kkuJyYSJyVJlCQAa8vLF6+yK380/eczSa12TZ766K+lpeBgFmdn5RtXqVGUb3wa8JqiPFtclKc+0LbO7P97Op9JMvlM8v6PfX2D/WB6Fx7W15/zQvwtXdlJjHVl7ybg/Qdg4cH6+t8DQnyovlUZfZ9kbwEAeW39cI9yAIQoC7AAm717CkwvmB5QEd5L2EuADiB7C8AeGB2UtN0fDgCo/3iAC7jw9uCugu6/udIENVtr/+KxAFzbAXUWHm49LW3CI3CedYiPMFdEdQMAPKeptw0DMLO345Go+7T0HDCOVeoECzO4/BnEYTgNzzG9pl43LI3AI+ev7Jw8pc4VsbvdLl77E6UZpD+HOAwApuZYune4//V/nNORA767wbCMNAZxGKUZAOgfPBRXNyCN4ovCYWlprqk4JrEBnqGUFsZHvr3vRN/me/rDwcDdbdS3AUAchhA9MuLaHnltkR37wU/h4uMXnM9ublgiC+pC6OfYEAtgYBADg2iDbbj6nq3V7d2aBYQR/P+F8ZGbOT3Sx0d6ue44xwus3yWAY7kW8RoHtnHgGvvWz79G/RQ4yo/C+MjkBOl7h+/qCfERlutiQzwLgFLq2tQxXMvwTNVuqO4vv8WCCGnPn8kJIkTAdbOhcCjEMQCoB8+lTsO1DNe0mUfz0db8eUOPZoRNThBb8/xjoxQ6oeFo6NF8FB0p2K5v7dKGY/Pzf4k47RuDcix4AAAAAElFTkSuQmCC");

                byte[] data2 = p.StringToCode("iVBORw0KGgoAAAANSUhEUgAAABUAAAAWCAIAAACg4UBvAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAsSAAALEgHS3X78AAAAFnRFWHRDcmVhdGlvbiBUaW1lADA1LzA1LzA35V+tVgAAABh0RVh0U29mdHdhcmUAQWRvYmUgRmlyZXdvcmtzT7MfTgAAA1BJREFUOI1tVE9o21Yc/iRLTqzYTkOyLYTqMBqsQ50l4FJo11K2QteymTkM6kAOG0sxHf0zNqjdw07byUxh0JJDGGyBUUh8KaSwW0kYa06BpetgdprDqBMTmjgk0pNk/X07KFZcOx9I6Pfxvt/ve4+nj6GUogOF8ZFOsvj4RSfJtOpbZSE2fu7jd0Xpvfrm6srvW0RRjm9Em8hnkpqiBOWrcvn762e12hV56nIrrylKPpMMSjaY/OV3c0IsBkBX1WqlIkqScGJs+huSOHtdiMWqlYquqgCEWOzCtTtHToPhQftcKpVLpRZnZ1+Vy7lUSlOUeVnOpVJfX7rkGymvrgbr2WAjfnv/DaBaqfgfQixmqCoAg5B6reaTrs75Fjjf/IUPe9eWl8+n0wNDQ1/Jcr1WO5dOl6anRy8OrTx58kkuJyYSJyVJlCQAa8vLF6+yK380/eczSa12TZ766K+lpeBgFmdn5RtXqVGUb3wa8JqiPFtclKc+0LbO7P97Op9JMvlM8v6PfX2D/WB6Fx7W15/zQvwtXdlJjHVl7ybg/Qdg4cH6+t8DQnyovlUZfZ9kbwEAeW39cI9yAIQoC7AAm717CkwvmB5QEd5L2EuADiB7C8AeGB2UtN0fDgCo/3iAC7jw9uCugu6/udIENVtr/+KxAFzbAXUWHm49LW3CI3CedYiPMFdEdQMAPKeptw0DMLO345Go+7T0HDCOVeoECzO4/BnEYTgNzzG9pl43LI3AI+ev7Jw8pc4VsbvdLl77E6UZpD+HOAwApuZYune4//V/nNORA767wbCMNAZxGKUZAOgfPBRXNyCN4ovCYWlprqk4JrEBnqGUFsZHvr3vRN/me/rDwcDdbdS3AUAchhA9MuLaHnltkR37wU/h4uMXnM9ublgiC+pC6OfYEAtgYBADg2iDbbj6nq3V7d2aBYQR/P+F8ZGbOT3Sx0d6ue44xwus3yWAY7kW8RoHtnHgGvvWz79G/RQ4yo/C+MjkBOl7h+/qCfERlutiQzwLgFLq2tQxXMvwTNVuqO4vv8WCCGnPn8kJIkTAdbOhcCjEMQCoB8+lTsO1DNe0mUfz0db8eUOPZoRNThBb8/xjoxQ6oeFo6NF8FB0p2K5v7dKGY/Pzf4k47RuDcix4AAAAAElFTkSuQmCC");

                byte[] data3 = p.StringToCode("iVBORw0KGgoAAAANSUhEUgAAABUAAAAWCAIAAACg4UBvAAAAA3NCSVQICAjb4U/gAAAACXBIWXMAAAsSAAALEgHS3X78AAAAFnRFWHRDcmVhdGlvbiBUaW1lADA1LzA1LzA35V+tVgAAABh0RVh0U29mdHdhcmUAQWRvYmUgRmlyZXdvcmtzT7MfTgAAA1BJREFUOI1tVE9o21Yc/iRLTqzYTkOyLYTqMBqsQ50l4FJo11K2QteymTkM6kAOG0sxHf0zNqjdw07byUxh0JJDGGyBUUh8KaSwW0kYa06BpetgdprDqBMTmjgk0pNk/X07KFZcOx9I6Pfxvt/ve4+nj6GUogOF8ZFOsvj4RSfJtOpbZSE2fu7jd0Xpvfrm6srvW0RRjm9Em8hnkpqiBOWrcvn762e12hV56nIrrylKPpMMSjaY/OV3c0IsBkBX1WqlIkqScGJs+huSOHtdiMWqlYquqgCEWOzCtTtHToPhQftcKpVLpRZnZ1+Vy7lUSlOUeVnOpVJfX7rkGymvrgbr2WAjfnv/DaBaqfgfQixmqCoAg5B6reaTrs75Fjjf/IUPe9eWl8+n0wNDQ1/Jcr1WO5dOl6anRy8OrTx58kkuJyYSJyVJlCQAa8vLF6+yK380/eczSa12TZ766K+lpeBgFmdn5RtXqVGUb3wa8JqiPFtclKc+0LbO7P97Op9JMvlM8v6PfX2D/WB6Fx7W15/zQvwtXdlJjHVl7ybg/Qdg4cH6+t8DQnyovlUZfZ9kbwEAeW39cI9yAIQoC7AAm717CkwvmB5QEd5L2EuADiB7C8AeGB2UtN0fDgCo/3iAC7jw9uCugu6/udIENVtr/+KxAFzbAXUWHm49LW3CI3CedYiPMFdEdQMAPKeptw0DMLO345Go+7T0HDCOVeoECzO4/BnEYTgNzzG9pl43LI3AI+ev7Jw8pc4VsbvdLl77E6UZpD+HOAwApuZYune4//V/nNORA767wbCMNAZxGKUZAOgfPBRXNyCN4ovCYWlprqk4JrEBnqGUFsZHvr3vRN/me/rDwcDdbdS3AUAchhA9MuLaHnltkR37wU/h4uMXnM9ublgiC+pC6OfYEAtgYBADg2iDbbj6nq3V7d2aBYQR/P+F8ZGbOT3Sx0d6ue44xwus3yWAY7kW8RoHtnHgGvvWz79G/RQ4yo/C+MjkBOl7h+/qCfERlutiQzwLgFLq2tQxXMvwTNVuqO4vv8WCCGnPn8kJIkTAdbOhcCjEMQCoB8+lTsO1DNe0mUfz0db8eUOPZoRNThBb8/xjoxQ6oeFo6NF8FB0p2K5v7dKGY/Pzf4k47RuDcix4AAAAAElFTkSuQmCC");

                p.CreateTable();

                p.GetTable().AddCell(p.CreateCell(p.GetImage(data1)));

                p.GetTable().AddCell(p.CreateCell(p.GetImage(data2)));

                p.GetTable().AddCell(p.CreateCell(p.GetImage(data3)));

                doc.Add(p.GetTable());

                doc.Close();

                pdfDocBytes = ms.ToArray();

                FileResult fileResult = new FileContentResult(pdfDocBytes, "application/pdf");

                fileResult.FileDownloadName = "Embroidery.pdf";

                return fileResult;

            }
        }


    }
}
