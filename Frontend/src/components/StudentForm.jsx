import { useState, useEffect } from 'react';
import { api } from '../api';

export default function StudentForm({ onCreated }) {
  const [form, setForm] = useState({ name: '', dob: '', gender: 'Male', currentGrade: '', parentId: '' });
  const [parents, setParents] = useState([]);
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    api.getParents().then(setParents).catch(() => {});
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setResult(null);
    try {
      const data = await api.createStudent(form);
      setResult(data);
      onCreated?.(data);
      setForm({ name: '', dob: '', gender: 'Male', currentGrade: '', parentId: '' });
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="card">
      <h3>Tạo Học Sinh</h3>
      <form onSubmit={handleSubmit}>
        <input placeholder="Họ tên" value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} required />
        <label style={{ fontSize: '0.85rem', color: '#666' }}>Ngày sinh</label>
        <input type="date" value={form.dob} onChange={e => setForm({ ...form, dob: e.target.value })} required />
        <select value={form.gender} onChange={e => setForm({ ...form, gender: e.target.value })}>
          <option value="Male">Nam</option>
          <option value="Female">Nữ</option>
        </select>
        <input placeholder="Lớp (vd: 3)" value={form.currentGrade} onChange={e => setForm({ ...form, currentGrade: e.target.value })} required />
        <select value={form.parentId} onChange={e => setForm({ ...form, parentId: e.target.value })} required>
          <option value="">-- Chọn Phụ Huynh --</option>
          {parents.map(p => (
            <option key={p.id} value={p.id}>{p.name} ({p.phone})</option>
          ))}
        </select>
        <button type="submit">Tạo</button>
      </form>
      {error && <p className="error">{error}</p>}
      {result && <p className="success">✓ Tạo thành công — {result.name} | PH: {result.parent?.name}</p>}
    </div>
  );
}
