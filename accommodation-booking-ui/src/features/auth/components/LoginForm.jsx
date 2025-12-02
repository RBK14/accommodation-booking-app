const LoginForm = ({ onSuccess }) => {
  const handleSubmit = (role) => {
    onSuccess({ role });
  };

  return (
    <div style={{ textAlign: 'center', padding: '20px' }}>
      <h2>Symulacja Logowania</h2>

      <button
        onClick={() => handleSubmit('Guest')}
        style={{
          padding: '15px',
          margin: '10px',
          background: '#0d6efd',
          color: 'white',
          border: 'none',
        }}
      >
        Zaloguj jako GOŚĆ
      </button>

      <button
        onClick={() => handleSubmit('Host')}
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
        onClick={() => handleSubmit('Admin')}
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
