import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import { LoginForm } from '../../components/auth';

const LoginPage = () => {
  const { login, userData } = useAuth();
  const navigate = useNavigate();

  const handleSuccess = (authResponse) => {
    const { id, accessToken } = authResponse;

    login(accessToken, id);

    const base64Url = accessToken.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    const claims = JSON.parse(jsonPayload);
    const role =
      claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || claims.role;

    if (role === 'Host') {
      navigate('/host');
    } else if (role === 'Admin') {
      navigate('/admin');
    } else {
      navigate('/');
    }
  };

  return <LoginForm onSuccess={handleSuccess} />;
};

export default LoginPage;
