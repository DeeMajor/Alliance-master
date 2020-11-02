using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAppointments();
        Appointment GetAppointments(int id);
        bool Insert(Appointment appointment);
        bool Update(Appointment appointment);
        bool Delete(Appointment appointment);
        IEnumerable<Appointment> Find(Func<Appointment, bool> prdicate);
    }
}
