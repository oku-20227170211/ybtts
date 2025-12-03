import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../api/axios'
import './AddStudent.css'

function AddStudent() {
  const [fullName, setFullName] = useState('')
  const [studentNumber, setStudentNumber] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      await api.post('/api/student', {
        fullName,
        studentNumber,
        password
      })
      navigate('/admin')
    } catch (err) {
      setError(err.response?.data?.message || 'Öğrenci ekleme başarısız')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="add-student-container">
      <h1>Yeni Öğrenci Ekle</h1>
      <form onSubmit={handleSubmit}>
        {error && <div className="error">{error}</div>}
        <input
          type="text"
          placeholder="Ad Soyad"
          value={fullName}
          onChange={(e) => setFullName(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Öğrenci Numarası"
          value={studentNumber}
          onChange={(e) => setStudentNumber(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Şifre"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button type="submit" disabled={loading}>{loading ? 'Ekleniyor...' : 'Öğrenci Ekle'}</button>
      </form>
    </div>
  )
}

export default AddStudent
