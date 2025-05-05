# Git Flow Guide
> **Objetivo**: Padronizar o fluxo de versionamento Git para garantir qualidade, rastreabilidade e colaboração eficiente no projeto.

## Branches Principais

| Branch    | Finalidade                                        | Protegida |
| --------- | ------------------------------------------------- | --------- |
| `main`    | Versão estável e liberada para entrega            | Sim       |
| `develop` | Integração contínua das funcionalidades validadas | Sim       |
## Branches de Trabalho

| Prefixo     | Uso                                              |
| ----------- | ------------------------------------------------ |
| `feat/`     | Novas Funcionalidades                            |
| `fix/`      | Correções de Bugs                                |
| `mod/`      | Ajustes Menores (ex: ajustes de regras, payload) |
| `css/`      | Estilização e Responsividade                     |
| `refactor/` | Reestruturação sem Alteração de Comportamento    |
| `test/`     | Testes Automatizados                             |
| `doc/`      | Documentação                                     |
| `hotfix/`   | Correções críticas na `main`                     |
> **Formato padrão de branch:**
```bash
<prefix>/<area><descricao>
```
## Fluxo de Trabalho
![[Fluxo de Trabalho|1000]]
## Regras Gerais
### Criação de branch
```bash
git checkout -b feat/backend/descricao
```
### Commit Convention
- `feat: add login button to header`
- `fix: correct barcode format on backend`
Veja o [[202505042130 Commits|Guia de Commits]]
### Subindo alterações
```bash
git add .
git commit -m "feat: create product listing endpoint"
git push -u origin feat/backend/product-endpoint
```
### Pull Request
- PR sempre para `develop`
- PR para `main` **somente após fechamento da sprint**
- Use o template [[202505041923 Pull Request Template]]
- Kennedy revisa e faz merge após testes
## Hotfix
- Criação direta a partir da `main`
- Ex: `hotfix/backend/deploy-error`
- Após PR e merge, fazer cherry-pick em `develop` (se necessário)
## Releases
- Releases são criadas na `main` com tag semântica: `v1.0.0`, `v1.1.0`, etc.
```bash
git checkout main
git tag -a v1.0.0 -m "Release inicial com módulo de busca"
git push origin v1.0.0
```