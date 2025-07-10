import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { API_BASE_URL } from '../config'; // Asegúrate de tener tu URL de la API en un config

const ProductDetail = () => {
    const { id } = useParams(); // Obtiene el 'id' de la URL
    const [product, setProduct] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                // Llama al nuevo endpoint de la API
                const response = await fetch(`${API_BASE_URL}/productos/${id}`);
                if (!response.ok) {
                    throw new Error('Producto no encontrado');
                }
                const data = await response.json();
                setProduct(data);
            } catch (error) {
                console.error("Error fetching product:", error);
                // Opcional: redirigir si el producto no se encuentra
                navigate('/'); 
            } finally {
                setLoading(false);
            }
        };

        fetchProduct();
    }, [id, navigate]); // Se ejecuta cada vez que el 'id' cambie

    const handleGoToCheckout = () => {
        navigate('/checkout', { state: { product } });
    };

    if (loading) {
        return <div>Cargando...</div>;
    }

    if (!product) {
        return <div>Producto no encontrado.</div>;
    }

    return (
        <div className="product-detail-container">
            <Link to="/" className="back-link">&#8592; Volver a la lista</Link>
            <div className="product-detail-card">
                <div className="product-detail-info">
                    <h1>{product.nombre}</h1>
                    {/* El resto de tu JSX aquí... */}
                    <p className="description">{product.descripcion}</p>
                    <p className="price">${product.precio.toFixed(2)}</p>
                    <p className="stock">{product.stock > 0 ? `${product.stock} unidades disponibles` : 'Sin stock'}</p>
                    <p className="brand">Marca: {product.marca}</p>
                    {/* Muestra el nombre de la categoría si existe */}
                    {product.categoriaNombre && <p className="category">Categoría: {product.categoriaNombre}</p>}
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