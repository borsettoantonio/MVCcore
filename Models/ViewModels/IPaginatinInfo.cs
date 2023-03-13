namespace MyCourse.Models.ViewModels
{
    public interface IPaginationInfo
    {
        int CurrentPage {get;}
        int TotalResults {get;}
        int ResultsPerPge {get;}
        string Search {get;}
         string OrderBy {get;}
         bool Ascending {get;}
         string Stato {get;}
    }
} 