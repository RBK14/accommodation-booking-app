import React from 'react';
import { Outlet } from 'react-router-dom';

const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column',
    fontFamily: 'Arial, sans-serif',
  },
  topbar: {
    background: '#0d6efd',
    color: '#ffffff',
    padding: '1rem 1.5rem',
    fontSize: '1.125rem',
    fontWeight: 'bold',
  },
  wrapper: {
    display: 'flex',
    flex: 1,
  },
  sidebar: {
    background: '#212529',
    color: '#ffffff',
    width: '250px',
    padding: '1.5rem 0',
    overflowY: 'auto',
  },
  main: {
    flex: 1,
    background: '#f1f3f5',
    padding: '2rem',
    overflowY: 'auto',
  },
};

const AdminLayout = () => {
  return (
    <div style={styles.container}>
      <header style={styles.topbar}></header>
      <div style={styles.wrapper}>
        <aside style={styles.sidebar}></aside>
        <main style={styles.main}>
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default AdminLayout;
