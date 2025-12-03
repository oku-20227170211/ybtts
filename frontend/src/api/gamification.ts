import api from './api';

/**
 * Liderlik tablosunu getir
 */

export const getLeaderboard = async (topN: number = 10) => {
  const response = await api.get(`/api/gamification/leaderboard?topN=${topN}`);
  return response.data;
};

export const getStudentStats = async (studentId: number) => {
  const response = await api.get(`/api/gamification/student/${studentId}/stats`);
  return response.data;
};

export const addPoints = async (studentId: number, points: number, reason: string) => {
  const response = await api.post('/api/gamification/add-points', {
    studentId,
    points,
    reason
  });
  return response.data;
};
