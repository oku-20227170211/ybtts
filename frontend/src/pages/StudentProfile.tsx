import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { getStudentStats } from '../api/gamification';
import Navbar from '../components/Navbar';

type Badge = {
  id: number;
  name: string;
  description: string;
  iconUrl: string;
};

type StudentStats = {
  studentId: number;
  fullName: string;
  score: number;
  level: number;
  nextLevelPoints: number;
  totalRequests: number;
  completedRequests: number;
  totalFeedbacks: number;
  badges: Badge[];
};

const StudentProfile: React.FC = () => {
  const { state } = useAuth();
  const [stats, setStats] = useState<StudentStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        if (!state.user?.id) return;
        const response = await getStudentStats(state.user.id);
        if (response.success) {
          setStats(response.data);
        }
      } catch (err: any) {
        setError(err?.response?.data?.message || 'İstatistikler yüklenemedi');
      } finally {
        setLoading(false);
      }
    };

    fetchStats();
  }, [state.user]);

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-xl text-gray-600">Yükleniyor...</div>
        </div>
      </>
    );
  }

  if (error || !stats) {
    return (
      <>
        <Navbar />
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-xl text-red-600">{error || 'Veri bulunamadı'}</div>
        </div>
      </>
    );
  }

  const progressPercentage = stats.nextLevelPoints > 0 
    ? (stats.score / stats.nextLevelPoints) * 100 
    : 100;

  return (
    <>
      <Navbar />
      <div className="min-h-screen bg-gradient-to-br from-purple-50 to-blue-50 py-12 px-6">
        <div className="max-w-5xl mx-auto">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-4xl font-bold text-gray-800 mb-2">
              Profilim 👤
            </h1>
            <p className="text-gray-600">Performansınızı ve başarılarınızı görüntüleyin</p>
          </div>

          {/* Stats Cards Grid */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
            {/* Level Card */}
            <div className="bg-gradient-to-br from-purple-500 to-purple-700 rounded-2xl shadow-xl p-6 text-white">
              <div className="flex items-center justify-between mb-4">
                <span className="text-lg font-semibold">Seviye</span>
                <span className="text-4xl">🎖️</span>
              </div>
              <div className="text-5xl font-bold mb-2">{stats.level}</div>
              <div className="text-purple-100 text-sm">
                {stats.nextLevelPoints > 0 ? `${stats.nextLevelPoints - stats.score} puan kaldı` : 'Maksimum seviye!'}
              </div>
            </div>

            {/* Score Card */}
            <div className="bg-gradient-to-br from-blue-500 to-blue-700 rounded-2xl shadow-xl p-6 text-white">
              <div className="flex items-center justify-between mb-4">
                <span className="text-lg font-semibold">Puan</span>
                <span className="text-4xl">⭐</span>
              </div>
              <div className="text-5xl font-bold mb-2">{stats.score}</div>
              <div className="text-blue-100 text-sm">Toplam kazanılan puan</div>
            </div>

            {/* Badges Card */}
            <div className="bg-gradient-to-br from-amber-500 to-amber-700 rounded-2xl shadow-xl p-6 text-white">
              <div className="flex items-center justify-between mb-4">
                <span className="text-lg font-semibold">Rozetler</span>
                <span className="text-4xl">🏆</span>
              </div>
              <div className="text-5xl font-bold mb-2">{stats.badges.length}</div>
              <div className="text-amber-100 text-sm">Kazanılan rozet sayısı</div>
            </div>
          </div>

          {/* Progress Bar */}
          {stats.nextLevelPoints > 0 && (
            <div className="bg-white rounded-2xl shadow-xl p-6 mb-8">
              <div className="flex items-center justify-between mb-3">
                <span className="text-gray-700 font-semibold">Sonraki Seviyeye İlerleme</span>
                <span className="text-gray-600 text-sm">
                  {stats.score} / {stats.nextLevelPoints}
                </span>
              </div>
              <div className="w-full bg-gray-200 rounded-full h-4 overflow-hidden">
                <div 
                  className="bg-gradient-to-r from-purple-500 to-blue-500 h-full rounded-full transition-all duration-500"
                  style={{ width: `${Math.min(progressPercentage, 100)}%` }}
                />
              </div>
            </div>
          )}

          {/* Activity Stats */}
          <div className="bg-white rounded-2xl shadow-xl p-6 mb-8">
            <h2 className="text-2xl font-bold text-gray-800 mb-6">Aktivite İstatistikleri</h2>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="text-center p-4 bg-gray-50 rounded-xl">
                <div className="text-3xl mb-2">📋</div>
                <div className="text-3xl font-bold text-gray-800">{stats.totalRequests}</div>
                <div className="text-gray-600 text-sm mt-1">Toplam Talep</div>
              </div>
              <div className="text-center p-4 bg-gray-50 rounded-xl">
                <div className="text-3xl mb-2">✅</div>
                <div className="text-3xl font-bold text-green-600">{stats.completedRequests}</div>
                <div className="text-gray-600 text-sm mt-1">Tamamlanan Talep</div>
              </div>
              <div className="text-center p-4 bg-gray-50 rounded-xl">
                <div className="text-3xl mb-2">💬</div>
                <div className="text-3xl font-bold text-blue-600">{stats.totalFeedbacks}</div>
                <div className="text-gray-600 text-sm mt-1">Geri Bildirim</div>
              </div>
            </div>
          </div>

          {/* Badges Section */}
          <div className="bg-white rounded-2xl shadow-xl p-6">
            <h2 className="text-2xl font-bold text-gray-800 mb-6">Rozetlerim 🏆</h2>
            {stats.badges.length === 0 ? (
              <div className="text-center py-12">
                <div className="text-6xl mb-4">🎯</div>
                <p className="text-gray-600 text-lg">Henüz rozet kazanmadınız</p>
                <p className="text-gray-500 text-sm mt-2">Talep oluşturun ve geri bildirim verin!</p>
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {stats.badges.map((badge) => (
                  <div 
                    key={badge.id}
                    className="bg-gradient-to-br from-amber-50 to-yellow-50 rounded-xl p-6 border-2 border-amber-200 hover:shadow-lg transition-all duration-300 transform hover:scale-105"
                  >
                    <div className="text-5xl text-center mb-3">{badge.iconUrl}</div>
                    <h3 className="text-lg font-bold text-gray-800 text-center mb-2">
                      {badge.name}
                    </h3>
                    <p className="text-gray-600 text-sm text-center">
                      {badge.description}
                    </p>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </>
  );
};

export default StudentProfile;
