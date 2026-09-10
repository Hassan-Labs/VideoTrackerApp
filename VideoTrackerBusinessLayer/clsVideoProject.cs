using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VideoTrackerDataAccessLayer;

namespace VideoTrackerBusinessLayer
{
    public class clsVideoProject
    {

        enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;

        public int ProjectID { get; set; }
        public string Title { get; set; }
        public int DurationInSecond { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int ClientID { get; set; }


        public clsVideoProject()
        {
            this.ProjectID = -1;
            this.Title = "";
            this.DurationInSecond = 0;
            this.Status = "";
            this.Price = 0;
            this.DeliveryDate = DateTime.MinValue;
            this.ClientID = -1;
            Mode = enMode.AddNew;
        }

        private clsVideoProject(int ProjectID, string Title, int DurationInSecond, string Status, decimal Price, DateTime DeliveryDate, int ClientID)
        {
            this.ProjectID = ProjectID;
            this.Title = Title;
            this.DurationInSecond = DurationInSecond;
            this.Status = Status;
            this.Price = Price;
            this.DeliveryDate = DeliveryDate;
            this.ClientID = ClientID;
            Mode = enMode.Update;
        }

        static public clsVideoProject Find(int ProjectID)
        {
            string Title = "", Status = "";
            decimal Price = 0;
            DateTime DeliveryTime = DateTime.MinValue;
            int DuratoinInSecond = -1, ClientID = -1;

            if (clsVideoProjectData.GetProjectInfoByID(ProjectID, ref Title, ref DuratoinInSecond, ref Status, ref Price, ref DeliveryTime, ref ClientID))
                {
                return new clsVideoProject(ProjectID, Title, DuratoinInSecond, Status, Price, DeliveryTime, ClientID);
            }
            else
                return null;
        }

        private bool _AddNewProject()
        {
            this.ProjectID = (clsVideoProjectData.AddNewProject(this.Title, this.DurationInSecond, this.Status, this.Price, this.DeliveryDate, this.ClientID));
            return (this.ProjectID != -1);
        }
        

        private bool _UpdateProject()
        {
            return (clsVideoProjectData.UpdateProject(this.ProjectID, this.Title, this.DurationInSecond, this.Status, this.Price, this.DeliveryDate, this.ClientID));
        }
    
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewProject())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return (_UpdateProject());
                    }
            }
            return false;
        }
    
        static public bool DeleteProject(int ProjectID)
        {
            return (clsVideoProjectData.DeleteProject(ProjectID));
        }
    
        static public DataTable GetAllProjects()
        {
            return clsVideoProjectData.GetAllProjects();
        }
   
        static public bool IsProjectExist(int ProjectID)
        {
            return (clsVideoProjectData.IsProjectExist( ProjectID));
        }
    } 



}

