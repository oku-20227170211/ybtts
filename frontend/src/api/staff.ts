import api from './api';

export const staffLogin = async (username: string, password: string) => {
  const response = await api.post('/api/staff/login', {
    username,
    password
  });
  return response.data;
};

export const getAllStaff = async () => {
  const response = await api.get('/api/staff');
  return response.data;
};

export const getAssignedRequests = async (staffId: number) => {
  const response = await api.get(`/api/staff/${staffId}/requests`);
  return response.data;
};

export const startProgress = async (requestId: number, staffId: number) => {
  const response = await api.put(`/api/staff/requests/${requestId}/start`, {
    staffId
  });
  return response.data;
};

export const completeRequest = async (requestId: number, staffId: number) => {
  const response = await api.put(`/api/staff/requests/${requestId}/complete`, {
    staffId
  });
  return response.data;
};

export const createStaff = async (
  fullName: string,
  username: string,
  password: string,
  email: string,
  specialization: string
) => {
  const response = await api.post('/api/staff', {
    fullName,
    username,
    password,
    email,
    specialization
  });
  return response.data;
};

export const assignRequestToStaff = async (requestId: number, staffId: number) => {
  const response = await api.put(`/api/requests/${requestId}/assign`, {
    staffId
  });
  return response.data;
};