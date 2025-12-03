import { useState, useEffect } from 'react'
import api from '../../api/axios'
import './MyRequests.css'

function MyRequests() {
  const [requests, setRequests] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [ratings, setRatings] = useState({})
  const [submittingId, setSubmittingId] = useState(null)
  const [studentProfile, setStudentProfile] = useState(null)
  const [leaderboard, setLeaderboard] = useState([])

  useEffect(() => {
    const fetchData = async () => {
      try {
        const studentToken = localStorage.getItem('studentToken')
        if (!studentToken) {
          console.error('Student token not found in localStorage')
          setRequests([])
          setStudentProfile(null)
          setLoading(false)
          return
        }

        const student = JSON.parse(studentToken)
        await Promise.all([
          fetchRequests(student.id),
          fetchStudentProfile(student.id),
          fetchLeaderboard()
        ])
        setError('')
      } catch (err) {
        console.error('Talepler yüklenemedi:', err)
        setRequests([])
        setError('Talepler yüklenemedi, lütfen tekrar deneyin.')
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [])

  const fetchRequests = async (studentId) => {
    const response = await api.get(`/api/requests/student/${studentId}`)
    setRequests(response.data.data || [])
  }

  const fetchStudentProfile = async (studentId) => {
    const response = await api.get(`/api/students/${studentId}`)
    setStudentProfile(response.data.data)
  }

  const fetchLeaderboard = async () => {
    const response = await api.get('/api/students/leaderboard')
    setLeaderboard(response.data.data || [])
  }

  const handleRatingChange = (requestId, value) => {
    setRatings((prev) => ({
      ...prev,
      [requestId]: Number(value)
    }))
  }

  const handleSubmitRating = async (requestId) => {
    const score = ratings[requestId] || 5
    setSubmittingId(requestId)
    setError('')

    try {
      const response = await api.post(`/api/requests/${requestId}/satisfaction`, {
        satisfactionScore: score
      })

      const updated = response.data.data
      setRequests((prev) =>
        prev.map((req) => (req.id === requestId ? { ...req, ...updated } : req))
      )
      const studentToken = localStorage.getItem('studentToken')
      if (studentToken) {
        const student = JSON.parse(studentToken)
        await fetchStudentProfile(student.id)
        await fetchLeaderboard()
      }
    } catch (err) {
      console.error('Memnuniyet puanı kaydedilemedi:', err)
      setError(err?.response?.data?.message || 'Memnuniyet puanı kaydedilemedi')
    } finally {
      setSubmittingId(null)
    }
  }

  if (loading) return <div>Yükleniyor...</div>

  return (
    <div className="my-requests-container">
      <h1>Taleplerim</h1>
      {studentProfile && (
        <div className="gamification-grid">
          <div className="stat-card">
            <p className="stat-label">Toplam Puan</p>
            <div className="stat-value">{studentProfile.points}</div>
            <p className="stat-subtitle">Seviye {studentProfile.level}</p>
          </div>
          <div className="stat-card">
            <p className="stat-label">Tamamlanan Talepler</p>
            <div className="stat-value">{studentProfile.completedRequests}</div>
            <p className="stat-subtitle">Toplam {studentProfile.totalRequests} talep</p>
          </div>
          <div className="stat-card">
            <p className="stat-label">Memnuniyet Ortalaması</p>
            <div className="stat-value">
              {studentProfile.averageSatisfaction ?? '-'}
            </div>
            <p className="stat-subtitle">1-5 arası skor</p>
          </div>
        </div>
      )}
      {error && <div className="error-message">{error}</div>}
      {requests.length === 0 ? (
        <p>Henüz talep oluşturulmadı.</p>
      ) : (
        <table className="requests-table">
          <thead>
            <tr>
              <th>Başlık</th>
              <th>Açıklama</th>
              <th>Durum</th>
              <th>Oluşturulma</th>
              <th>Tamamlanma</th>
              <th>Memnuniyet</th>
            </tr>
          </thead>
          <tbody>
            {requests.map((req) => (
              <tr key={req.id}>
                <td>{req.title}</td>
                <td>{req.description}</td>
                <td>
                  {req.status === 'Pending' && 'Bekleniyor'}
                  {req.status === 'Approved' && 'Onaylandı'}
                  {req.status === 'Rejected' && 'Reddedildi'}
                  {req.status === 'Completed' && 'Tamamlandı'}
                </td>
                <td>{new Date(req.createdAt).toLocaleDateString('tr-TR')}</td>
                <td>
                  {req.completedAt
                    ? new Date(req.completedAt).toLocaleDateString('tr-TR')
                    : '-'}
                </td>
                <td>
                  {req.status === 'Completed' ? (
                    req.satisfactionScore ? (
                      <span className="rating-readonly">{req.satisfactionScore}/5</span>
                    ) : (
                      <div className="rating-input">
                        <select
                          value={ratings[req.id] || 5}
                          onChange={(e) => handleRatingChange(req.id, e.target.value)}
                        >
                          {[1, 2, 3, 4, 5].map((score) => (
                            <option key={score} value={score}>
                              {score}
                            </option>
                          ))}
                        </select>
                        <button
                          onClick={() => handleSubmitRating(req.id)}
                          disabled={submittingId === req.id}
                        >
                          {submittingId === req.id ? 'Kaydediliyor...' : 'Kaydet'}
                        </button>
                      </div>
                    )
                  ) : (
                    '-'
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {leaderboard.length > 0 && (
        <div className="leaderboard">
          <h2>En Aktif Öğrenciler</h2>
          <table className="leaderboard-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Öğrenci</th>
                <th>Puan</th>
                <th>Seviye</th>
                <th>Tamamlanan</th>
              </tr>
            </thead>
            <tbody>
              {leaderboard.map((entry, index) => (
                <tr key={entry.id}>
                  <td>{index + 1}</td>
                  <td>
                    <div className="leader-name">{entry.fullName}</div>
                    <div className="leader-subtext">{entry.studentNumber}</div>
                  </td>
                  <td>{entry.points}</td>
                  <td>{entry.level}</td>
                  <td>{entry.completedRequests}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}

export default MyRequests
