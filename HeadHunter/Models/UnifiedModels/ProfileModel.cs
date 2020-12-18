using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HeadHunter.Models.Entity;
namespace HeadHunter.Models.UnifiedModels
{
    public class ProfileModel
    {
        public string fullName { get; set; }
        public string mail { get; set; }
        public string githubUsername { get; set; }
        public string stackOverUsername { get; set; }
        public double? githubRepos { get; set; }
        public double? githubStars { get; set; }
        public List<string> githubLanguages { get; set; }//
        public double? githubLanguageCount { get; set; }
        public int StackOverReputation { get; set; }
        public string StackOverAnswers { get; set; }
        public string StackOverQuestions { get; set; }
        public string StackOverReached { get; set; }
        public string StackOverMostTag { get; set; }
    }
}