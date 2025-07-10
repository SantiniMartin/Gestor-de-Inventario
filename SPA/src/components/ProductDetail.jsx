import React from 'react';
import { useLocation, Link, Navigate, useNavigate } from 'react-router-dom';

const ProductDetail = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { product } = location.state || {};

    const handleGoToCheckout = () => {
        navigate('/checkout', { state: { product } });
    };

    // If the user navigates directly to this URL, the state will be undefined.
    // In that case, we redirect them to the homepage.
    if (!product) {
        return <Navigate to="/" replace />;
    }

    return (
        <div className="product-detail-container">
            <Link to="/" className="back-link">&#8592; Volver a la lista</Link>
            <div className="product-detail-card">
                <div className="product-detail-info">
                    <h1>{product.nombre}</h1>
                    <p className="description">{product.descripcion}</p>
                    <p className="price">${product.precio.toFixed(2)}</p>
                    <p className="stock">{product.stock > 0 ? `${product.stock} unidades disponibles` : 'Sin stock'}</p>
                    <p className="brand">Marca: {product.marca}</p>
                    <button 
                        className="buy-button" 
                        disabled={product.stock === 0} 
                        onClick={handleGoToCheckout}
                    >
                        {product.stock > 0 ? 'Comprar' : 'No disponible'}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ProductDetail;
