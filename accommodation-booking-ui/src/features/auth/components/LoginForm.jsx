import useAuth from '../../../hooks/useAuth';
import { useNavigate } from 'react-router-dom';

const LoginForm = () => {
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSimulateLogin = (role) => {
    login(role);

    // if (role === 'Admin') navigate('/admin');
    // if (role === 'Host') navigate('/host');

    console.log(`Zalogowano jako: ${role}`);
  };

  return (
    <div style={{ textAlign: 'center', padding: '20px' }}>
      <h2>Symulacja Logowania</h2>

      <button
        onClick={() => handleSimulateLogin('Host')}
        style={{
          padding: '15px',
          margin: '10px',
          background: '#0d6efd',
          color: 'white',
          border: 'none',
        }}
      >
        Zaloguj jako GOSPODARZ
      </button>

      <button
        onClick={() => handleSimulateLogin('Admin')}
        style={{
          padding: '15px',
          margin: '10px',
          background: '#212529',
          color: 'white',
          border: 'none',
        }}
      >
        Zaloguj jako ADMIN
      </button>
    </div>
  );
};

export default LoginForm;
