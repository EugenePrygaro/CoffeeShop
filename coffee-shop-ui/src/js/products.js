/**
 * @param {Array} products - Масив об'єктів товарів [ {id, name, price, desc}, ... ]
 * @returns {Array} - Масив об'єктів з доданим полем img
 */

export const richProductsWithImg = products => {
  if (!products || !Array.isArray(products)) return [];
  return products.map((product, index) => {
    const searchTerm = product.name
      .toLowerCase()
      .replace(/[^a-zа-яєіїґ\s]/gi, '')
      .trim()
      .replace(/\s+/g, ',');

    const categoryMap = { 1: 'Coffee', 2: 'Tea', 3: 'Accessories' };
    const categoryName =
      product.category?.name || categoryMap[product.categoryId] || 'Product';

    return {
      ...product,
      categoryLabel: categoryName,
      img: `https://loremflickr.com/600/400/coffee,${searchTerm}/all?lock=${product.id}`,
    };
  });
};
