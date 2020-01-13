//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace WC.WinerSchool.BOL
{
    [DataContract(IsReference = true)]
    public partial class tblcertificate_config: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'Id' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private int _id;
    
        [DataMember]
        public string CertificateName
        {
            get { return _certificateName; }
            set
            {
                if (_certificateName != value)
                {
                    _certificateName = value;
                    OnPropertyChanged("CertificateName");
                }
            }
        }
        private string _certificateName;
    
        [DataMember]
        public sbyte CertificateType
        {
            get { return _certificateType; }
            set
            {
                if (_certificateType != value)
                {
                    _certificateType = value;
                    OnPropertyChanged("CertificateType");
                }
            }
        }
        private sbyte _certificateType;
    
        [DataMember]
        public string ExamName
        {
            get { return _examName; }
            set
            {
                if (_examName != value)
                {
                    _examName = value;
                    OnPropertyChanged("ExamName");
                }
            }
        }
        private string _examName;
    
        [DataMember]
        public string BoardofExam
        {
            get { return _boardofExam; }
            set
            {
                if (_boardofExam != value)
                {
                    _boardofExam = value;
                    OnPropertyChanged("BoardofExam");
                }
            }
        }
        private string _boardofExam;
    
        [DataMember]
        public bool IsRollNumberNeed
        {
            get { return _isRollNumberNeed; }
            set
            {
                if (_isRollNumberNeed != value)
                {
                    _isRollNumberNeed = value;
                    OnPropertyChanged("IsRollNumberNeed");
                }
            }
        }
        private bool _isRollNumberNeed;
    
        [DataMember]
        public string RollNum1
        {
            get { return _rollNum1; }
            set
            {
                if (_rollNum1 != value)
                {
                    _rollNum1 = value;
                    OnPropertyChanged("RollNum1");
                }
            }
        }
        private string _rollNum1;
    
        [DataMember]
        public string RollNum2
        {
            get { return _rollNum2; }
            set
            {
                if (_rollNum2 != value)
                {
                    _rollNum2 = value;
                    OnPropertyChanged("RollNum2");
                }
            }
        }
        private string _rollNum2;
    
        [DataMember]
        public string StudentBehaviourSentences
        {
            get { return _studentBehaviourSentences; }
            set
            {
                if (_studentBehaviourSentences != value)
                {
                    _studentBehaviourSentences = value;
                    OnPropertyChanged("StudentBehaviourSentences");
                }
            }
        }
        private string _studentBehaviourSentences;
    
        [DataMember]
        public string FooterLeft
        {
            get { return _footerLeft; }
            set
            {
                if (_footerLeft != value)
                {
                    _footerLeft = value;
                    OnPropertyChanged("FooterLeft");
                }
            }
        }
        private string _footerLeft;
    
        [DataMember]
        public string FooterCenter
        {
            get { return _footerCenter; }
            set
            {
                if (_footerCenter != value)
                {
                    _footerCenter = value;
                    OnPropertyChanged("FooterCenter");
                }
            }
        }
        private string _footerCenter;
    
        [DataMember]
        public string FooterRight
        {
            get { return _footerRight; }
            set
            {
                if (_footerRight != value)
                {
                    _footerRight = value;
                    OnPropertyChanged("FooterRight");
                }
            }
        }
        private string _footerRight;
    
        [DataMember]
        public Nullable<short> Need_Address
        {
            get { return _need_Address; }
            set
            {
                if (_need_Address != value)
                {
                    _need_Address = value;
                    OnPropertyChanged("Need_Address");
                }
            }
        }
        private Nullable<short> _need_Address;
    
        [DataMember]
        public string Need_Estd
        {
            get { return _need_Estd; }
            set
            {
                if (_need_Estd != value)
                {
                    _need_Estd = value;
                    OnPropertyChanged("Need_Estd");
                }
            }
        }
        private string _need_Estd;
    
        [DataMember]
        public string Need_SchoolCode
        {
            get { return _need_SchoolCode; }
            set
            {
                if (_need_SchoolCode != value)
                {
                    _need_SchoolCode = value;
                    OnPropertyChanged("Need_SchoolCode");
                }
            }
        }
        private string _need_SchoolCode;
    
        [DataMember]
        public sbyte CertificateStatus
        {
            get { return _certificateStatus; }
            set
            {
                if (_certificateStatus != value)
                {
                    _certificateStatus = value;
                    OnPropertyChanged("CertificateStatus");
                }
            }
        }
        private sbyte _certificateStatus;
    
        [DataMember]
        public string CertificateContent
        {
            get { return _certificateContent; }
            set
            {
                if (_certificateContent != value)
                {
                    _certificateContent = value;
                    OnPropertyChanged("CertificateContent");
                }
            }
        }
        private string _certificateContent;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
        }

        #endregion
    }
}