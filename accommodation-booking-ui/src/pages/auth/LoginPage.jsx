import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import { LoginForm } from '../../components/auth';

const LoginPage = () => {
  const { login } = useAuth();

  const navigate = useNavigate();

  const handleSuccess = (authResponse) => {
    const { id, accessToken } = authResponse;

    login(accessToken, id);

    setTimeout(() => {
      const storedAuth = JSON.parse(localStorage.getItem('auth') || '{}');
      const role = storedAuth.role;

      if (role === 'Host') navigate('/host');
      else if (role === 'Admin') navigate('/admin');
      else navigate('/');
    }, 100);
  };

  return <LoginForm onSuccess={handleSuccess} />;
};

export default LoginPage;
