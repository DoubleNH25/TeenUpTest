import { useState, useEffect } from 'react';
import { api } from '../api';

export default function RegisterForm({ selectedClass, onClear }) {
  const [students, setStudents] = useState([]);
  const [subscriptions, setSubscriptions] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState('');
  const [selectedSub, setSelectedSub] = useState('');
  const [nextSessionAt, setNextSessionAt] = useState('');
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    api.getStudents().then(setStudents).catch(() => {});
  }, []);

  // Load subscriptions when student changes
  useEffect(() => {
    if (!selectedStudent) { setSubscriptions([]); setSelectedSub(''); return; }
    api.getSubscriptionsByStudent(selectedStudent)
      .then(data => { setSubscriptions(data); setSelectedSub(''); })
      .catch(() => setSubscriptions([]));
  }, [selectedStudent]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setResult(null);
    try {
      const body = {
        studentId: selectedStudent,
        subscriptionId: selectedSub,
        nextSessionAt: nextSessionAt ? new Date(nextSessionAt).toISOString() : null,
      };
      const data = await api.registerStudent(selectedClass.id, body);
      setResult(data);
      setSelectedStudent(''); setSelectedSub(''); setNextSessionAt('');
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="card highlight">
      <div style={{ display: 'flex', justifyContent: 'space-between' }}>
        <h3>Đăng Ký Lớp Học</h3>
        {onClear && <button onClick={onClear} style={{ background: '#888' }}>✕ Bỏ chọn</button>}
      </div>

      {selectedClass
        ? <p style={{ marginBottom: '0.75rem' }}>
            Lớp: <strong>{selectedClass.name}</strong> — {selectedClass.dayOfWeek} {selectedClass.timeSlot} — GV: {selectedClass.teacherName}
            &nbsp;<span className={selectedClass.currentStudents >= selectedClass.maxStudents ? 'inactive' : 'active'}>
              ({selectedClass.currentStudents}/{selectedClass.maxStudents})
            </span>
          </p>
        : <p className="hint" style={{ marginBottom: '0.75rem' }}>← Click vào một lớp trong bảng lịch để chọn</p>
      }

      <form onSubmit={handleSubmit}>
        <select value={selectedStudent} onChange={e => setSelectedStudent(e.target.value)} required>
          <option value="">-- Chọn Học Sinh --</option>
          {students.map(s => (
            <option key={s.id} value={s.id}>{s.name} (Lớp {s.currentGrade})</option>
          ))}
        </select>

        <select value={selectedSub} onChange={e => setSelectedSub(e.target.value)} required disabled={!selectedStudent}>
          <option value="">-- Chọn Gói Học --</option>
          {subscriptions.map(s => (
            <option key={s.id} value={s.id} disabled={!s.isActive}>
              {s.packageName} — còn {s.remainingSessions} buổi {!s.isActive ? '(hết hạn)' : ''}
            </option>
          ))}
        </select>

        <label style={{ fontSize: '0.85rem', color: '#666' }}>Thời gian buổi học (dùng để tính hoàn buổi khi hủy)</label>
        <input type="datetime-local" value={nextSessionAt} onChange={e => setNextSessionAt(e.target.value)} />

        <button type="submit" disabled={!selectedClass || !selectedStudent || !selectedSub}>Đăng Ký</button>
      </form>

      {error && <p className="error">{error}</p>}
      {result && <p className="success">✓ Đăng ký thành công — Reg ID: <code>{result.id}</code></p>}
    </div>
  );
}
