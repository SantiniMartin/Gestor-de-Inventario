import React from 'react';
import { Routes, Route, Link, useNavigate } from 'react-router-dom';
import ProductList from './components/ProductList';
import ProductDetail from './components/ProductDetail';
import CreateProduct from './components/CreateProduct';
import Checkout from './components/Checkout';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword';
import ResetPassword from './components/ResetPassword';
import ProtectedRoute from './components/ProtectedRoute';
import { useAuth } from './hooks/useAuth';
import './App.css';

function App() {
  const { currentUser, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="App">
      <header className="App-header">
        <div className="header-content">
          <Link to="/" className="header-logo"><h1>Mi Tienda</h1></Link>
          <nav className="header-nav">
            {currentUser ? (
              <>
                <span className="welcome-user">Hola, {currentUser.nombreUsuario}</span>
                <button onClick={handleLogout} className="auth-button-logout">Cerrar Sesión</button>
              </>
            ) : (
              <>
                <Link to="/login" className="header-link">Login</Link>
                <Link to="/register" className="header-link">Register</Link>
              </>
            )}
          </nav>
        </div>
      </header>
      <main>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<ResetPassword />} />
          <Route 
            path="/"
            element={
              <ProtectedRoute>
                <ProductList />
              </ProtectedRoute>
            }
          />
          <Route 
            path="/admin/create-product"
            element={
              <ProtectedRoute>
                <CreateProduct />
              </ProtectedRoute>
            }
          />
          <Route 
            path="/product/:id"
            element={
              <ProtectedRoute>
                <ProductDetail />
              </ProtectedRoute>
            }
          />
          <Route 
            path="/checkout"
            element={
              <ProtectedRoute>
                <Checkout />
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>
    </div>
  );
}

export default App
