import api from './api';

export const createFeedback = async (
  requestId: number,
  studentId: number,
  rating: number,
  comment: string
) => {
  const response = await api.post('/api/feedback', {
    requestId,
    studentId,
    rating,
    comment
  });
  return response.data;
};

export const getFeedbackByRequest = async (requestId: number) => {
  const response = await api.get(`/api/feedback/request/${requestId}`);
  return response.data;
};