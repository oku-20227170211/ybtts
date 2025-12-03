import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5000', // güncelle
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})

// request interceptor: token ekleme
api.interceptors.request.use((config) => {
  const raw = localStorage.getItem('ybtts_auth')
  if (raw) {
    try {
      const { token } = JSON.parse(raw)
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`
      }
    } catch {
      // ignore
    }
  }
  return config
})

export default api
