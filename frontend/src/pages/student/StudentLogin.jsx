import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../api/axios'
import './StudentLogin.css'

function StudentLogin() {
  const [studentNumber, setStudentNumber] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  const handleLogin = async (e) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    
    try {
      const response = await api.post('/api/student-auth/login', {
        studentNumber,
        password
      })
      
      if (response.data.success) {
        localStorage.setItem('studentToken', JSON.stringify(response.data.student))
        navigate('/student')
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Giriş başarısız')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleLogin}>
        <h1>Öğrenci Girişi</h1>
        {error && <div className="error">{error}</div>}
        <input
          type="text"
          placeholder="Öğrenci Numarası"
          value={studentNumber}
          onChange={(e) => setStudentNumber(e.target.value)}
          required
          disabled={loading}
        />
        <input
          type="password"
          placeholder="Şifre"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          disabled={loading}
        />
        <button type="submit" disabled={loading}>
          {loading ? 'Giriş yapılıyor...' : 'Giriş Yap'}
        </button>
      </form>
    </div>
  )
}

export default StudentLogin
