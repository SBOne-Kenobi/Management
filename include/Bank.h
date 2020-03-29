#ifndef MANAGEMENT_BANK_H
#define MANAGEMENT_BANK_H

#include <vector>
#include <random>
#include <algorithm>

namespace Management {

  struct DemandOffer {
    double UMat; //ед. сырья на игрока.
    int MinPrice; //минимальная цена покупки сырья.
    double UProd; //ед. продукции на игрока.
    int MaxPrice; //максимальная цела продажи продукции.
  };

  struct Demand {
    int UMat;
    int Price;
    int Priority;

    bool operator<(const Demand &x) const;
  };

  struct Offer {
    int UProd;
    int Price;
    int Priority;

    bool operator<(const Offer &x) const;
  };

  class Bank {
  private:
    std::vector<std::discrete_distribution<>> probOfPriceChange; //вероятности изменения уровня цен.
    std::vector<DemandOffer> priceLevelInfo; //инофрмация по уровням цен.
    int price_level; //текущий уровень цен.
    int materials; //остаток лимита материалов.
    int product; //остаток лимита продукции.

    std::mt19937 gen; //для рандома

  public:
    explicit Bank(int N); //принимает общее кол-во игроков

    ~Bank() = default;

    [[nodiscard]] int getPriceLevel() const;

    void setNewPriceLevel(int N);

    [[nodiscard]] DemandOffer getInfo() const;

    std::vector<int> requestOfMat(const std::vector<Demand> &demand); //возвращает индексы принятых запросов.

    std::vector<int> requestOfProd(const std::vector<Offer> &offer); //возвращает индексы принятыз запросов.

    friend class Management;
  };
}


#endif
