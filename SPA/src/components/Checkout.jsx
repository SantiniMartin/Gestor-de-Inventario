import React from 'react';
import { useLocation, useNavigate, Link, Navigate } from 'react-router-dom';
import { API_BASE_URL } from '../config';

const Checkout = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const product = location.state?.product;

    const handleConfirmPurchase = async () => {
        if (!product || product.stock <= 0) {
            alert('Este producto no está disponible.');
            return;
        }

        const newStock = product.stock - 1;
        const productUpdate = { stock: newStock };

        try {
            const response = await fetch(`${API_BASE_URL}/productos/${product.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(productUpdate),
            });

            if (!response.ok) {
                const errorData = await response.text();
                throw new Error(`Error al procesar la compra: ${errorData}`);
            }

            alert('¡Compra realizada con éxito!');
            // Redirect to homepage after purchase
            navigate('/');

        } catch (error) {
            console.error('Error en la compra:', error);
            alert(error.message);
        }
    };

    if (!product) {
        // If no product was passed, redirect to home
        return <Navigate to="/" replace />;
    }

    return (
        <div className="checkout-container">
            <h1>Finalizar Compra</h1>
            <div className="product-detail-card">
                <div className="product-detail-info">
                    <h2>{product.nombre}</h2>
                    <p className="price">Precio: ${product.precio.toFixed(2)}</p>
                    <p className="stock">Quedarán {product.stock - 1} unidades disponibles.</p>
                </div>
            </div>
            <div className="payment-section">
                <h3>Método de Pago</h3>
                {/* Placeholder for payment options */}
                <p>Aquí irían las opciones de pago (Tarjeta, PayPal, etc.)</p>
                <button className="buy-button" onClick={handleConfirmPurchase}>
                    Confirmar Compra
                </button>
            </div>
            <Link to={`/`} className="back-link">Cancelar y volver</Link>
        </div>
    );
};

export default Checkout;
