using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FrameworklessWebApp2
{
    public static class Html
    {
        public static string Wrap(string message, string htmlTags)
        {
            
            var htmlTagMatches = Regex.Matches(htmlTags, @"<\/?[A-Za-z0-9]+>");
            
            var htmlTagsList = new List<string>();
            
            foreach (Match match in htmlTagMatches)
            {
                foreach (Capture capture in match.Captures)
                {
                    htmlTagsList.Add(capture.Value);
                }
            }
            
            return htmlTagsList[0] + message + htmlTagsList[1];  // needs exception handling 
        }
 
    }
}