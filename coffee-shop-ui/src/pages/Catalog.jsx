// src/pages/Catalog.jsx
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { initMobileMenu } from '../js/mobile-menu';
import { Header } from '../components/Header';
import { Footer } from '../components/Footer';
import { richProductsWithImg } from '../js/products';

const Catalog = () => {
  // Тимчасові дані (mock data) 
  const [products, setProducts] = useState([]);
  const [cartItems, setCartItems] = useState([]); 
  const [isCartOpen, setIsCartOpen] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await axios.get('http://localhost:5254/api/product');
        
        const enrichedData = richProductsWithImg(response.data);
        setProducts(enrichedData);
      } catch (error) {
        console.error("Помилка при завантаженні товарів:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  
  useEffect(() => {
    const menu = initMobileMenu();
    return () => menu?.cleanup();
  }, []);
  useEffect(() => {
    if (isCartOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'unset';
    }
  }, [isCartOpen]);

  const addToCart = (product) => {
    setCartItems(prev => [...prev, product]);
  };

  const handleCheckout = async () => {
    if (cartItems.length === 0) return;

    const productIds = cartItems.map(item => item.id);

    const orderPayload = {
      customerId: 1, // Тимчасово 1 ID клієнта (Іван Франко)
      productIds: productIds
    };

    try {
      const response = await axios.post('http://localhost:5254/api/order', orderPayload);
      
      alert(`Успіх! ${response.data.message} (Номер: ${response.data.orderId})`);
      setCartItems([]);
      setIsCartOpen(false);

      window.location.reload();

    } catch (error) {
      if (error.response && error.response.data) {
        alert(`Помилка: ${error.response.data}`);
      } else {
        alert("Сталася помилка при оформленні замовлення. Перевірте підключення.");
      }
      console.error("Checkout error:", error);
    }
  };

  if (loading) return <div className="container">Loading our best coffee...</div>;

  return (
    <>
      <Header />

      <main>
        <section className="catalog">
          <div className="container catalog-container">
            <div className='catalog-top'>
              <h1 className="catalog-title">Our goods</h1>
              <button className="hero-button" onClick={() => setIsCartOpen(true)}>Cart ({cartItems.length})</button>
            </div>
            
            <ul className="catalog-list">
              {products.map(product => (
                <li key={product.id} className="catalog-item">
                    <div className="catalog-img-wrapper">
                      {/* Стікер категорії */}
                      <div className="category-sticker">{product.categoryLabel}</div>
                      <img className="catalog-image" src={product.img} alt={product.name} />
                    </div>

                    <div className="catalog-content">
                      <h3 className="catalog-title-list">{product.name}</h3>
                      <p className="catalog-list-text">{product.description}</p>
                      {/* Візуальне обсмаження (тільки для кави) */}
                      {product.categoryId === 1 && (
                        <div className="roast-level">
                          <span>Roast:</span>
                          {[...Array(5)].map((_, i) => (
                            <span key={i} className={`bean ${i < product.roastLevel ? 'active' : ''}`}>☕</span>
                          ))}
                        </div>
                      )}
                      
                      <div className="catalog-meta">
                        <span className="stock-info">
                          {product.stockQuantity > 0 ? `In stock: ${product.stockQuantity}` : 'Out of stock'}
                        </span>
                        <span className="weight-info">{product.weight}g</span>
                      </div>

                      <div className='catalog-list-button'>
                        <span className='catalog-list-price'>{product.price} uah</span>
                        <button 
                          className="hero-button" 
                          disabled={product.stockQuantity === 0}
                          onClick={() => addToCart(product)}
                        >
                          {product.stockQuantity > 0 ? 'buy now' : 'sold out'}
                        </button>
                      </div>
                    </div>
                  </li>
              ))}
            </ul>
          </div>
        </section>
      </main>
        {isCartOpen && (
        <div className="cart-backdrop" onClick={() => setIsCartOpen(false)}>
          <div className="cart-modal" onClick={e => e.stopPropagation()}>
            <button className="cart-close-btn" onClick={() => setIsCartOpen(false)}>×</button>
            
            <h2 className="quality-title">Your Cart</h2>
            
            {cartItems.length === 0 ? (
              <p className="catalog-list-text">Cart is empty</p>
            ) : (
              <ul className="cart-items-list">
                {cartItems.map((item, index) => (
                  <li key={index} className="cart-item">
                    <img src={item.img} alt={item.name} className="cart-item-img" />
                    <div className="cart-item-info">
                      <h4>{item.name}</h4>
                      <p>{item.price} uah</p>
                    </div>
                  </li>
                ))}
              </ul>
            )}

            {cartItems.length > 0 && (
              <div className="cart-footer">
                <div className="cart-total-text">
                  <span>Total:</span>
                  <span>{cartItems.reduce((acc, curr) => acc + curr.price, 0)} uah</span>
                </div>
                <button className="hero-button" onClick={handleCheckout}>Checkout</button>
              </div>
            )}
          </div>
        </div>
  )}

      <Footer />
    </>
  );
};

export default Catalog;