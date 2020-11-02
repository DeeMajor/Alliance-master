using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private DatabaseService<Appointment> _databaseService;
        public AppointmentRepository(DatabaseService<Appointment> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(Appointment appointment)
        {
            return _databaseService.Delete(appointment);
        }

        public IEnumerable<Appointment> Find(Func<Appointment, bool> predicate)
        {
            return _databaseService.Find(predicate);
        }

        public List<Appointment> GetAppointments()
        {
            return _databaseService.Get().Include(a => a.Managers).Include(a => a.Timeslot).ToList();
        }

        public Appointment GetAppointments(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(Appointment appointment)
        {
            return _databaseService.Insert(appointment);
        }

        public bool Update(Appointment appointment)
        {
            return _databaseService.Update(appointment);
        }
    }
}