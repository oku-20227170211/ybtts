import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

const Navbar = () => {
  const { state, setState } = useAuth();
  const navigate = useNavigate();

  const logout = () => {
    setState({ user: null, token: null });
    localStorage.removeItem("auth");
    navigate("/login");
  };

  return (
    <nav className="bg-gray-800 text-white px-6 py-3 flex justify-between items-center">
      <h1 className="font-semibold text-lg">
        {state.user?.role === "Admin" ? "Admin Panel" : "Student Panel"}
      </h1>

      <button
        onClick={logout}
        className="bg-red-500 px-4 py-1 rounded hover:bg-red-600"
      >
        Logout
      </button>
    </nav>
  );
};

export default Navbar;
