import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../../api/axios'
import './CreateRequest.css'

function CreateRequest() {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const navigate = useNavigate()

  const isFormValid = title.trim() && description.trim()

  const handleSubmit = async (e) => {
    e.preventDefault()
    
    if (!isFormValid) {
      setError('Başlık ve açıklama gereklidir')
      return
    }

    setLoading(true)
    setError('')
    
    try {
      const student = JSON.parse(localStorage.getItem('studentToken'))
      console.log('Sending request with:', { title, description, studentId: student.id })
      
      await api.post('/api/requests', {
        title,
        description,
        studentId: student.id
      })
      
      console.log('Talep başarıyla oluşturuldu')
      navigate('/student/requests')
    } catch (err) {
      console.error('Talep oluşturma hatası:', err)
      setError(err?.response?.data?.message || 'Talep oluşturulamadı')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="create-request-container">
      <h1>Yeni Talep Oluştur</h1>
      
      {error && <div className="error-message">{error}</div>}
      
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Talep Başlığı"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <textarea
          placeholder="Talep Açıklaması"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
          rows="5"
        />
        <button 
          type="submit" 
          disabled={loading || !isFormValid}
        >
          {loading ? 'Gönderiliyor...' : 'Talep Gönder'}
        </button>
      </form>
    </div>
  )
}

export default CreateRequest
