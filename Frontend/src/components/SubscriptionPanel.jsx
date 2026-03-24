import { useState, useEffect } from 'react';
import { api } from '../api';

export default function SubscriptionPanel() {
  const [students, setStudents] = useState([]);
  const [createForm, setCreateForm] = useState({ studentId: '', packageName: '', startDate: '', endDate: '', totalSessions: '' });
  const [createResult, setCreateResult] = useState(null);
  const [createError, setCreateError] = useState('');

  const [lookupStudentId, setLookupStudentId] = useState('');
  const [subList, setSubList] = useState([]);
  const [selectedSub, setSelectedSub] = useState(null);
  const [subError, setSubError] = useState('');

  useEffect(() => {
    api.getStudents().then(setStudents).catch(() => {});
  }, []);

  const handleCreate = async (e) => {
    e.preventDefault();
    setCreateError(''); setCreateResult(null);
    try {
      const data = await api.createSubscription({ ...createForm, totalSessions: parseInt(createForm.totalSessions) });
      setCreateResult(data);
      setCreateForm({ studentId: '', packageName: '', startDate: '', endDate: '', totalSessions: '' });
      // Refresh list if same student is being viewed
      if (lookupStudentId === data.studentId) loadSubList(data.studentId);
    } catch (err) {
      setCreateError(err.message);
    }
  };

  const loadSubList = async (studentId) => {
    setSubError(''); setSubList([]); setSelectedSub(null);
    try {
      const data = await api.getSubscriptionsByStudent(studentId);
      setSubList(data);
    } catch (err) {
      setSubError(err.message);
    }
  };

  const handleLookupStudent = (e) => {
    const id = e.target.value;
    setLookupStudentId(id);
    if (id) loadSubList(id);
    else { setSubList([]); setSelectedSub(null); }
  };

  const handleUse = async () => {
    if (!selectedSub) return;
    setSubError('');
    try {
      const data = await api.useSession(selectedSub.id);
      setSelectedSub(data);
      setSubList(prev => prev.map(s => s.id === data.id ? data : s));
    } catch (err) {
      setSubError(err.message);
    }
  };

  return (
    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
      {/* Create */}
      <div className="card">
        <h3>Tạo Gói Học</h3>
        <form onSubmit={handleCreate}>
          <select value={createForm.studentId} onChange={e => setCreateForm({ ...createForm, studentId: e.target.value })} required>
            <option value="">-- Chọn Học Sinh --</option>
            {students.map(s => <option key={s.id} value={s.id}>{s.name} (Lớp {s.currentGrade})</option>)}
          </select>
          <input placeholder="Tên gói (vd: Basic 10)" value={createForm.packageName} onChange={e => setCreateForm({ ...createForm, packageName: e.target.value })} required />
          <label style={{ fontSize: '0.85rem', color: '#666' }}>Ngày bắt đầu</label>
          <input type="date" value={createForm.startDate} onChange={e => setCreateForm({ ...createForm, startDate: e.target.value })} required />
          <label style={{ fontSize: '0.85rem', color: '#666' }}>Ngày hết hạn</label>
          <input type="date" value={createForm.endDate} onChange={e => setCreateForm({ ...createForm, endDate: e.target.value })} required />
          <input type="number" placeholder="Tổng số buổi" value={createForm.totalSessions} onChange={e => setCreateForm({ ...createForm, totalSessions: e.target.value })} required min="1" />
          <button type="submit">Tạo</button>
        </form>
        {createError && <p className="error">{createError}</p>}
        {createResult && <p className="success">✓ {createResult.packageName} — {createResult.totalSessions} buổi cho {createResult.studentName}</p>}
      </div>

      {/* Lookup + Use */}
      <div className="card">
        <h3>Xem / Dùng Buổi</h3>
        <select value={lookupStudentId} onChange={handleLookupStudent}>
          <option value="">-- Chọn Học Sinh --</option>
          {students.map(s => <option key={s.id} value={s.id}>{s.name} (Lớp {s.currentGrade})</option>)}
        </select>

        {subList.length > 0 && (
          <select value={selectedSub?.id || ''} onChange={e => setSelectedSub(subList.find(s => s.id === e.target.value) || null)} style={{ marginTop: '0.5rem' }}>
            <option value="">-- Chọn Gói Học --</option>
            {subList.map(s => (
              <option key={s.id} value={s.id}>
                {s.packageName} — còn {s.remainingSessions}/{s.totalSessions} buổi {s.isActive ? '' : '(hết hạn)'}
              </option>
            ))}
          </select>
        )}

        {subError && <p className="error">{subError}</p>}

        {selectedSub && (
          <div className="sub-info">
            <p><strong>{selectedSub.packageName}</strong> — {selectedSub.studentName}</p>
            <p>Hạn: {new Date(selectedSub.endDate).toLocaleDateString('vi-VN')}</p>
            <div className="session-bar">
              <div className="session-used" style={{ width: `${(selectedSub.usedSessions / selectedSub.totalSessions) * 100}%` }} />
            </div>
            <p>{selectedSub.usedSessions} / {selectedSub.totalSessions} buổi đã dùng — còn <strong>{selectedSub.remainingSessions}</strong></p>
            <p>Trạng thái: <span className={selectedSub.isActive ? 'active' : 'inactive'}>{selectedSub.isActive ? '✓ Còn hiệu lực' : '✗ Hết hạn/buổi'}</span></p>
            <button onClick={handleUse} disabled={!selectedSub.isActive}>Dùng 1 buổi</button>
          </div>
        )}
      </div>
    </div>
  );
}
