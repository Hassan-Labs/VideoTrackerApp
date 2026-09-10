using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using VideoTrackerDataAccessLayer;
namespace VideoTrackerBusinessLayer
{
    public class clsClient
    {
        enum enMode { AddNew = 0 , Update = 1};
        enMode Mode = enMode.AddNew;
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Platform { get; set; }
        public string ContactInfo { get; set; }


        public clsClient()
        {
            this.ClientId = -1;
            this.ClientName = "";
            this.Platform = "";
            this.ContactInfo = "";
            Mode = enMode.AddNew;
        }

        private clsClient(int ClientID,string ClientName , string Platform, string ContactInfo)
        {
            this.ClientId = ClientID;
            this.ClientName = ClientName;
            this.Platform = Platform;
            this.ContactInfo = ContactInfo;
            Mode = enMode.Update;
        }

        static public clsClient Find(int ClientID)
        {
            string ClientName = "", Platform = "", ContactInfo = "";
            if (clsClientData.GetClientInfoByID(ClientID, ref ClientName, ref Platform, ref ContactInfo))
            {
                return new clsClient(ClientID, ClientName, Platform, ContactInfo);
            }
            else
                return null;
        }

        static public clsClient Find(string ClientName)
        {
            int ClientID = -1;
            string Platform = "", ContactInfo = "";
            if (clsClientData.GetClientInfoByName(ClientName, ref ClientID, ref Platform, ref ContactInfo))
            {
                return new clsClient(ClientID, ClientName, Platform, ContactInfo);
            }
            else
                return null;
        }

        private bool _AddNewClient()
        {
            this.ClientId = (clsClientData.AddNewClient(this.ClientName, this.Platform, this.ContactInfo));
            return (ClientId != -1);
        }

        private bool _UpdateClient()
        {
            return (clsClientData.UpdateClient(this.ClientId, this.ClientName, this.Platform, this.ContactInfo));
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewClient())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;

                    }
                case enMode.Update:
                    {
                        return (_UpdateClient());
                    }
            }
            return false;

        }

        static public bool DeleteClient(int ClientID)
        {
            return (clsClientData.DeleteClient(ClientID));
        }

        static public DataTable GetAllClients()
        {
            return clsClientData.GetAllClients();
        }

        static public bool IsClientExist(int ClientID)
        {
            return clsClientData.IsClientstExist(ClientID);
        }

    }
}
