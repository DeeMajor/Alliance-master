using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository _appointmentRepository;
        private IManagerBuildingRepository _managerBuildingRepository;
        private IManagerTimeSlotRepository _managerTimeSlotRepository;
        private IManagerRepository _managerRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IManagerBuildingRepository managerBuildingRepository, 
            IManagerTimeSlotRepository managerTimeSlotRepository,IManagerRepository managerRepository)
        {
            _appointmentRepository = appointmentRepository;
            _managerBuildingRepository = managerBuildingRepository;
            _managerTimeSlotRepository = managerTimeSlotRepository;
            _managerRepository = managerRepository;
        }

        public bool CheckAppoinment(Appointment appointment)
        {
            var appoimtments = _appointmentRepository.GetAppointments().ToList();
            bool result = false;
            foreach(var iten in appoimtments)
            {
                if (iten.ManagerId == appointment.ManagerId)
                {
                    if(iten.TimeSlotID==appointment.TimeSlotID && iten.ADate == appointment.ADate)
                    {
                        result= true;
                    }
                }
            }
            return result;

        }

        public bool checkDate(DateTime bookingForDate)
        {
            bool result = false;
            if (bookingForDate > DateTime.Now)
            {
                result = true;
            }
            return result;
        }

        public bool Delete(Appointment appointment)
        {
            return _appointmentRepository.Delete(appointment);
        }

        public IEnumerable<Appointment> Find(Func<Appointment, bool> prdicate)
        {
            return _appointmentRepository.Find(prdicate);
        }

        public List<Appointment> GetAppointments()
        {
            return _appointmentRepository.GetAppointments().ToList();
        }

        public Appointment GetAppointments(int id)
        {
            return _appointmentRepository.GetAppointments(id);
        }

        public string getBuildingAddress(int? managerId)
        {
            throw new NotImplementedException();
        }

        public int getReferenceManager(int? managerId)
        {
            var referenceManager = (from bm in _managerBuildingRepository.GetAll()
                                    where bm.BuildingId == managerId
                                    select bm.ManagerId).FirstOrDefault();  // this should return the 
                                    //corresponding manager id from the association tablle;
            return referenceManager;
        }

        //public List<int> getReferenceTimeSlot(int managerId)
        //{
        //    var managerEmail = (from m in _managerRepository.GetManagers()
        //                        where m.ManagerId == managerId
        //                        select m.Email).FirstOrDefault();

        //    var referenceId = (from tm in _managerTimeSlotRepository.GetManagerTimeSlots()
        //                       where tm.ManagerEmail == managerEmail
        //                       select tm.TimeSlotId).ToList();

        //    return referenceId;
        //}

        public bool Insert(Appointment appointment)
        {
            return _appointmentRepository.Insert(appointment);
        }

        public bool Update(Appointment appointment)
        {
            return _appointmentRepository.Update(appointment);
        }

        int IAppointmentService.getReferenceTimeSlot(int managerId)
        {
            var managerEmail = (from m in _managerRepository.GetManagers()
                                where m.ManagerId == managerId
                                select m.Email).FirstOrDefault();

            var referenceId = (from tm in _managerTimeSlotRepository.GetManagerTimeSlots()
                               where tm.ManagerEmail == managerEmail
                               select tm.TimeSlotId).FirstOrDefault();

            return Convert.ToInt32(referenceId);
        }
    }
}