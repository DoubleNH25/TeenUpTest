import { useState, useEffect } from 'react';
import { api } from '../api';

const DAYS = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
const DAY_LABELS = { Monday: 'Thứ 2', Tuesday: 'Thứ 3', Wednesday: 'Thứ 4', Thursday: 'Thứ 5', Friday: 'Thứ 6', Saturday: 'Thứ 7', Sunday: 'CN' };

export default function ClassSchedule({ onSelectClass }) {
  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(false);

  const loadAll = async () => {
    setLoading(true);
    try {
      const data = await api.getClasses();
      setClasses(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { loadAll(); }, []);

  const byDay = DAYS.reduce((acc, d) => {
    acc[d] = classes.filter(c => c.dayOfWeek === d);
    return acc;
  }, {});

  return (
    <div className="card">
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h3>Lịch Lớp Học (Tuần)</h3>
        <button onClick={loadAll}>↻ Tải lại</button>
      </div>
      {loading && <p>Đang tải...</p>}
      <div className="schedule-grid">
        {DAYS.map(day => (
          <div key={day} className="day-col">
            <div className="day-header">{DAY_LABELS[day]}</div>
            {byDay[day].length === 0
              ? <div className="empty-slot">—</div>
              : byDay[day].map(cls => (
                <div key={cls.id} className="class-slot" onClick={() => onSelectClass?.(cls)} title="Click để đăng ký">
                  <strong>{cls.name}</strong>
                  <span>{cls.timeSlot}</span>
                  <span>{cls.teacherName}</span>
                  <span className={cls.currentStudents >= cls.maxStudents ? 'full' : ''}>
                    {cls.currentStudents}/{cls.maxStudents}
                  </span>
                </div>
              ))
            }
          </div>
        ))}
      </div>
    </div>
  );
}
