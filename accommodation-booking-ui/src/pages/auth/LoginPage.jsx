import { useNavigate, useLocation } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import LoginForm from '../../features/auth/components/LoginForm';

const LoginPage = () => {
  const { login } = useAuth();

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname;

  const handleSuccess = (res) => {
    const { role } = res;

    login(role);

    console.log(`Zalogowano jako: ${role}`);

    if (from) {
      navigate(from, { replace: true });
      return;
    }

    if (role === 'Guest') navigate('/');
    if (role === 'Host') navigate('/host');
    if (role === 'Admin') navigate('/admin');
  };

  return <LoginForm onSuccess={handleSuccess} />;
};

export default LoginPage;
