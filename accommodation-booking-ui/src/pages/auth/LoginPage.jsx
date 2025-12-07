import { useNavigate, useLocation } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import LoginForm from '../../features/auth/components/LoginForm';

const LoginPage = () => {
  const { login } = useAuth();

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname;

  const handleSuccess = (authResponse) => {
    const { id, accessToken } = authResponse;

    login(accessToken, id);

    if (from) {
      navigate(from, { replace: true });
      return;
    }

    setTimeout(() => {
      const storedAuth = JSON.parse(localStorage.getItem('auth') || '{}');
      const role = storedAuth.role;

      if (role === 'Guest') navigate('/');
      else if (role === 'Host') navigate('/host');
      else if (role === 'Admin') navigate('/admin');
      else navigate('/');
    }, 100);
  };

  return <LoginForm onSuccess={handleSuccess} />;
};

export default LoginPage;
