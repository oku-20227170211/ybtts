# Değişiklikleri Diğer Ortama Aktarma Kılavuzu

Bu repo için uzaktaki son değişiklikleri başka bir makinede veya VS Code ortamında almak için izleyebileceğiniz adımlar:

1. **Uzak depoyu kontrol et:**
   ```bash
   git remote -v
   ```
   Eğer doğru GitHub adresi görünmüyorsa ekleyin veya güncelleyin:
   ```bash
   git remote add origin git@github.com:oku-20227170211/ybtts.git
   ```

2. **Güncel referansları çek:**
   ```bash
   git fetch origin
   ```

3. **İlgili dalı alın:** Bu repo üzerinde değişiklikler `work` dalında saklanıyor. Aynı dalı yerelde oluşturup güncelleyin:
   ```bash
   git switch work    # dal yoksa otomatik oluşturur
   git pull origin work
   ```
   veya dalı açıkça oluşturmak isterseniz:
   ```bash
   git fetch origin work:work
   git switch work
   ```

4. **Kendi dalınızda birleşim yapın (isteğe bağlı):** Eğer `main` üzerinde çalışıyorsanız, `work` dalını kendi dalınıza katabilirsiniz:
   ```bash
   git checkout main
   git pull origin main
   git merge work
   ```

5. **Yerel değişiklikler varsa önce saklayın/commit edin:** Çekme veya birleştirme öncesi çatışma yaşamamak için:
   ```bash
   git status
   git stash           # veya değişiklikleri commit'leyin
   ```

Bu adımlar, burada yapılan güncel commit'leri kendi ortamınıza doğrudan getirmenizi sağlar.
