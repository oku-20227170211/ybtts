import { useEffect, useState } from 'react';
import { getLeaderboard } from '../api/gamification';
import Navbar from '../components/Navbar';

type LeaderboardEntry = {
  rank: number;
  studentId: number;
  fullName: string;
  studentNumber: string;
  score: number;
  level: number;
  badgeCount: number;
};

const Leaderboard: React.FC = () => {
  const [leaderboard, setLeaderboard] = useState<LeaderboardEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLeaderboard = async () => {
      try {
        const response = await getLeaderboard(10);
        if (response.success) {
          setLeaderboard(response.data);
        }
      } catch (err: any) {
        setError(err?.response?.data?.message || 'Liderlik tablosu yüklenemedi');
      } finally {
        setLoading(false);
      }
    };

    fetchLeaderboard();
  }, []);

  const getMedalEmoji = (rank: number) => {
    switch(rank) {
      case 1: return '🥇';
      case 2: return '🥈';
      case 3: return '🥉';
      default: return `#${rank}`;
    }
  };

  const getRankColor = (rank: number) => {
    switch(rank) {
      case 1: return 'from-yellow-400 to-yellow-600';
      case 2: return 'from-gray-300 to-gray-500';
      case 3: return 'from-orange-400 to-orange-600';
      default: return 'from-blue-400 to-purple-600';
    }
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <div className="flex items-center justify-center min-h-screen\">
          <div className="text-xl text-gray-600\">Yükleniyor...</div>
        </div>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="flex items-center justify-center min-h-screen\">
          <div className="text-xl text-red-600\">{error}</div>
        </div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="min-h-screen bg-gradient-to-br from-purple-50 to-blue-50 py-12 px-6\">
        <div className="max-w-5xl mx-auto\">
          {/* Header */}
          <div className="text-center mb-12\">
            <div className="text-6xl mb-4\">🏆</div>
            <h1 className="text-5xl font-bold bg-gradient-to-r from-yellow-500 via-purple-600 to-blue-600 bg-clip-text text-transparent mb-4\">
              Liderlik Tablosu
            </h1>
            <p className="text-gray-600 text-lg\">
              En aktif ve başarılı öğrenciler
            </p>
          </div>

          {/* Top 3 Podium */}
          {leaderboard.length >= 3 && (
            <div className="flex items-end justify-center gap-4 mb-12\">
              {/* 2nd Place */}
              <div className="flex flex-col items-center\">
                <div className="bg-gradient-to-br from-gray-300 to-gray-500 text-white rounded-2xl p-6 w-48 shadow-2xl transform hover:scale-105 transition-all\">
                  <div className="text-4xl text-center mb-2\">🥈</div>
                  <div className="text-xl font-bold text-center mb-2\">{leaderboard[1].fullName}</div>
                  <div className="text-center text-sm opacity-90 mb-3\">{leaderboard[1].studentNumber}</div>
                  <div className="text-center\">
                    <div className="text-2xl font-bold\">{leaderboard[1].score}</div>
                    <div className="text-sm opacity-90\">puan</div>
                  </div>
                </div>
                <div className="bg-gray-400 h-24 w-48 rounded-t-xl mt-2\"></div>
              </div>

              {/* 1st Place */}
              <div className="flex flex-col items-center\">
                <div className="bg-gradient-to-br from-yellow-400 to-yellow-600 text-white rounded-2xl p-6 w-52 shadow-2xl transform hover:scale-105 transition-all\">
                  <div className="text-5xl text-center mb-2\">🥇</div>
                  <div className="text-2xl font-bold text-center mb-2\">{leaderboard[0].fullName}</div>
                  <div className="text-center text-sm opacity-90 mb-3\">{leaderboard[0].studentNumber}</div>
                  <div className="text-center\">
                    <div className="text-3xl font-bold\">{leaderboard[0].score}</div>
                    <div className="text-sm opacity-90\">puan</div>
                  </div>
                </div>
                <div className="bg-yellow-500 h-32 w-52 rounded-t-xl mt-2\"></div>
              </div>

              {/* 3rd Place */}
              <div className="flex flex-col items-center\">
                <div className="bg-gradient-to-br from-orange-400 to-orange-600 text-white rounded-2xl p-6 w-48 shadow-2xl transform hover:scale-105 transition-all\">
                  <div className="text-4xl text-center mb-2\">🥉</div>
                  <div className="text-xl font-bold text-center mb-2\">{leaderboard[2].fullName}</div>
                  <div className="text-center text-sm opacity-90 mb-3\">{leaderboard[2].studentNumber}</div>
                  <div className="text-center\">
                    <div className="text-2xl font-bold\">{leaderboard[2].score}</div>
                    <div className="text-sm opacity-90\">puan</div>
                  </div>
                </div>
                <div className="bg-orange-500 h-16 w-48 rounded-t-xl mt-2\"></div>
              </div>
            </div>
          )}

          {/* Full Leaderboard Table */}
          <div className="bg-white rounded-2xl shadow-xl overflow-hidden\">
            <div className="bg-gradient-to-r from-purple-600 to-blue-600 p-6\">
              <h2 className="text-2xl font-bold text-white\">Tüm Sıralama</h2>
            </div>
            <div className="overflow-x-auto\">
              <table className="w-full\">
                <thead className="bg-gray-50\">
                  <tr>
                    <th className="px-6 py-4 text-left text-sm font-semibold text-gray-700">Sıra</th>
                    <th className="px-6 py-4 text-left text-sm font-semibold text-gray-700">Öğrenci</th>
                    <th className="px-6 py-4 text-left text-sm font-semibold text-gray-700">Öğrenci No</th>
                    <th className="px-6 py-4 text-center text-sm font-semibold text-gray-700">Seviye</th>
                    <th className="px-6 py-4 text-center text-sm font-semibold text-gray-700">Puan</th>
                    <th className="px-6 py-4 text-center text-sm font-semibold text-gray-700">Rozetler</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {leaderboard.map((entry) =>(
                    <tr
                      key={entry.studentId}
                      className={`hover:bg-gray-50 transition-colors ${
                        entry.rank <= 3 ? 'bg-gradient-to-r from-yellow-50 to-orange-50' : ''
                      }`}
                    >
                      <td className="px-6 py-4\">
                        <div className={`text-2xl font-bold ${
                          entry.rank <= 3 ? 'text-3xl' : 'text-gray-600'
                        }`}>
                          {getMedalEmoji(entry.rank)}
                        </div>
                      </td>
                      <td className="px-6 py-4\">
                        <div className="font-semibold text-gray-800\">{entry.fullName}</div>
                      </td>
                      <td className="px-6 py-4\">
                        <div className="text-gray-600\">{entry.studentNumber}</div>
                      </td>
                      <td className="px-6 py-4 text-center\">
                        <div className={`inline-block px-4 py-2 rounded-full bg-gradient-to-r ${getRankColor(entry.rank)} text-white font-bold`}>
                          Level {entry.level}
                        </div>
                      </td>
                      <td className="px-6 py-4 text-center\">
                        <div className="text-xl font-bold text-purple-600">{entry.score}</div>
                      </td>
                      <td className="px-6 py-4 text-center\">
                        <div className="flex items-center justify-center gap-1\">
                          <span className="text-xl\">🏆</span>
                          <span className="text-lg font-semibold text-gray-700">{entry.badgeCount}</span>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>

          {leaderboard.length === 0 && (
            <div className="text-center py-20">
              <div className="text-6xl mb-4">🎯</div>
              <p className="text-gray-600 text-xl">Henüz liderlik tablosunda kimse yok</p>
              <p className="text-gray-500 mt-2">İlk sıralarda yer almak için talep oluşturun!</p>
            </div>
          )}
        </div>
      </div>
    </> 
  );
};

export default Leaderboard;