// src/pages/Catalog.jsx
import React, { useEffect, useState } from 'react';
import { initMobileMenu } from '../js/mobile-menu';
import { Header } from '../components/Header';
import { Footer } from '../components/Footer';

const Catalog = () => {
  // Тимчасові дані (mock data) для верстки, поки бекенд не готовий
  const [products] = useState([
    { id: 1, name: "Ethiopia Arabica", price: 350, img: "/img/quality/img_05.jpg", desc: "Світле обсмаження, нотки цитрусу." },
    { id: 2, name: "Colombia Dark", price: 420, img: "/img/quality/img_04.jpg", desc: "Темне обсмаження, шоколадний смак." },
    { id: 3, name: "Kenya Special", price: 380, img: "/img/quality/img_03.jpg", desc: "Насичений аромат та винна кислинка." },
    { id: 4, name: "Green Tea Sencha", price: 250, img: "/img/experience/img_11.jpg", desc: "Класичний японський зелений чай." },
  ]);

  useEffect(() => {
    const menu = initMobileMenu();
    return () => menu?.cleanup();
  }, []);

  return (
    <>
      <Header />

      <main>
        <section className="catalog">
          <div className="container catalog-container">
            <h1 className="catalog-title">Our goods</h1>
            
            <ul className="catalog-list">
              {products.map(product => (
                <li key={product.id} className="catalog-item">
                  <img className="catalog-image" src={product.img} alt={product.name} />
                  <h3 className="catalog-title-list">{product.name}</h3>
                  <p className="catalog-list-text">{product.desc}</p>
                  <div className='catalog-list-button'>
                    <span className='catalog-list-price'>{product.price} uah</span>
                    <button className="hero-button">shop now</button>
                  </div>
                </li>
              ))}
            </ul>
          </div>
        </section>
      </main>

      <Footer />
    </>
  );
};

export default Catalog;