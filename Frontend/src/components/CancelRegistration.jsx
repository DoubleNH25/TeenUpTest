import { useState, useEffect } from 'react';
import { api } from '../api';

export default function CancelRegistration() {
  const [students, setStudents] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState('');
  const [regId, setRegId] = useState('');
  const [result, setResult] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    api.getStudents().then(setStudents).catch(() => {});
  }, []);

  const handleCancel = async (e) => {
    e.preventDefault();
    setError(''); setResult('');
    try {
      const data = await api.cancelRegistration(regId);
      setResult(data.message);
      setRegId('');
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="card">
      <h3>Hủy Đăng Ký</h3>
      <p className="hint" style={{ marginBottom: '0.5rem' }}>
        Sau khi đăng ký thành công, copy Registration ID từ thông báo ✓ ở form đăng ký bên trên.
      </p>
      <form onSubmit={handleCancel} style={{ flexDirection: 'row', display: 'flex', gap: '0.5rem' }}>
        <input
          placeholder="Registration ID"
          value={regId}
          onChange={e => setRegId(e.target.value)}
          required
          style={{ flex: 1 }}
        />
        <button type="submit" style={{ background: '#e74c3c', whiteSpace: 'nowrap' }}>Hủy đăng ký</button>
      </form>
      {error && <p className="error">{error}</p>}
      {result && <p className="success">✓ {result}</p>}
    </div>
  );
}
