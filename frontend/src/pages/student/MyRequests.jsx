import { useState, useEffect } from 'react'
import api from '../../api/axios'
import './MyRequests.css'

function MyRequests() {
  const [requests, setRequests] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchRequests = async () => {
      try {
        const studentToken = localStorage.getItem('studentToken')
        if (!studentToken) {
          console.error('Student token not found in localStorage')
          setRequests([])
          setLoading(false)
          return
        }
        
        const student = JSON.parse(studentToken)
        console.log('Student ID from localStorage:', student.id)
        
        const response = await api.get(`/api/requests/student/${student.id}`)
        console.log('API Response:', response.data)
        setRequests(response.data.data || [])
      } catch (err) {
        console.error('Talepler yüklenemedi:', err)
        setRequests([])
      } finally {
        setLoading(false)
      }
    }

    fetchRequests()
  }, [])

  if (loading) return <div>Yükleniyor...</div>

  return (
    <div className="my-requests-container">
      <h1>Taleplerim</h1>
      {requests.length === 0 ? (
        <p>Henüz talep oluşturulmadı.</p>
      ) : (
        <table className="requests-table">
          <thead>
            <tr>
              <th>Başlık</th>
              <th>Açıklama</th>
              <th>Durum</th>
              <th>Tarih</th>
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
                </td>
                <td>{new Date(req.createdAt).toLocaleDateString('tr-TR')}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}

export default MyRequests
