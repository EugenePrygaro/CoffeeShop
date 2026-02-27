// src/pages/Catalog.jsx
import React, { useEffect, useState } from 'react';
import { initMobileMenu } from '../js/mobile-menu';
import { Header } from '../components/Header';
import { Footer } from '../components/Footer';
import { richProductsWithImg } from '../js/products';

const Catalog = () => {
  // Тимчасові дані (mock data) 
  const [products, setProducts] = useState([]);
  const [cartItems, setCartItems] = useState([]); 
  const [isCartOpen, setIsCartOpen] = useState(false);
  
  console.log(richProductsWithImg());

  useEffect(() => {
    setProducts(richProductsWithImg());

   },[]);
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
                  <img className="catalog-image" src={product.img} alt={product.name} />
                  <h3 className="catalog-title-list">{product.name}</h3>
                  <p className="catalog-list-text">{product.desc}</p>
                  <div className='catalog-list-button'>
                    <span className='catalog-list-price'>{product.price} uah</span>
                    <button className="hero-button" onClick={() => addToCart(product)}>shop</button>
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
                <button className="hero-button" style={{ width: '100%' }}>Checkout</button>
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