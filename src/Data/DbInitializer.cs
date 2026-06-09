using DuolingoTechPlatform.Helpers;
using DuolingoTechPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Courses.AnyAsync()) return;

            // ── Cursos ──────────────────────────────────────────────
            var expoDeepDive = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Expo Deep Dive",
                Description = "Domine o desenvolvimento mobile com Expo e React Native do zero ao avançado."
            };
            var reactNativeAvancado = new Course
            {
                Id = Guid.NewGuid(),
                Title = "React Native Avançado",
                Description = "Técnicas avançadas de performance, hooks e arquitetura para apps React Native."
            };
            var awsParaApps = new Course
            {
                Id = Guid.NewGuid(),
                Title = "AWS para App Devs",
                Description = "Integre serviços AWS no seu app mobile: S3, Cognito, DynamoDB e muito mais."
            };

            context.Courses.AddRange(expoDeepDive, reactNativeAvancado, awsParaApps);

            // ── Módulos ─────────────────────────────────────────────
            var modExpo1 = new Module { Id = Guid.NewGuid(), CourseId = expoDeepDive.Id, Title = "Fundamentos do Expo", Order = 1 };
            var modExpo2 = new Module { Id = Guid.NewGuid(), CourseId = expoDeepDive.Id, Title = "Navegação e Roteamento", Order = 2 };

            var modRN1 = new Module { Id = Guid.NewGuid(), CourseId = reactNativeAvancado.Id, Title = "Hooks e Performance", Order = 1 };
            var modRN2 = new Module { Id = Guid.NewGuid(), CourseId = reactNativeAvancado.Id, Title = "Gerenciamento de Estado", Order = 2 };

            var modAWS1 = new Module { Id = Guid.NewGuid(), CourseId = awsParaApps.Id, Title = "AWS Fundamentos", Order = 1 };
            var modAWS2 = new Module { Id = Guid.NewGuid(), CourseId = awsParaApps.Id, Title = "Cognito e Autenticação", Order = 2 };

            context.Modules.AddRange(modExpo1, modExpo2, modRN1, modRN2, modAWS1, modAWS2);

            // ── Lições ──────────────────────────────────────────────
            var lesExpo1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "Introdução ao Expo", Content = "O que é Expo, suas vantagens e como configurar o ambiente de desenvolvimento.", Order = 1 };
            var lesExpo1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "Componentes Básicos", Content = "View, Text, Image, ScrollView e TouchableOpacity no React Native.", Order = 2 };
            var lesExpo1_3 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "StyleSheet e Flexbox", Content = "Estilização com StyleSheet.create e layout com Flexbox.", Order = 3 };

            var lesExpo2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo2.Id, Title = "Expo Router", Content = "Roteamento baseado em arquivos com Expo Router v3.", Order = 1 };
            var lesExpo2_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo2.Id, Title = "Stack e Tab Navigation", Content = "Navegação por pilha e abas com Expo Router.", Order = 2 };

            var lesRN1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN1.Id, Title = "useCallback e useMemo", Content = "Otimização de renders com memoização de funções e valores.", Order = 1 };
            var lesRN1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN1.Id, Title = "React.memo", Content = "Memoização de componentes para evitar re-renders desnecessários.", Order = 2 };

            var lesRN2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN2.Id, Title = "Context API", Content = "Gerenciamento de estado global com Context API e useContext.", Order = 1 };
            var lesRN2_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN2.Id, Title = "Zustand e Redux Toolkit", Content = "Alternativas modernas ao Context para estado global.", Order = 2 };

            var lesAWS1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS1.Id, Title = "IAM e Segurança", Content = "Identidade e controle de acesso com IAM na AWS.", Order = 1 };
            var lesAWS1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS1.Id, Title = "Amazon S3 Básico", Content = "Upload e gerenciamento de arquivos com Amazon S3.", Order = 2 };

            var lesAWS2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS2.Id, Title = "Configurando Cognito", Content = "User Pools e autenticação com Amazon Cognito.", Order = 1 };
            var lesAWS2_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS2.Id, Title = "Tokens JWT no Cognito", Content = "Access Token, ID Token e Refresh Token com Cognito.", Order = 2 };

            context.Lessons.AddRange(
                lesExpo1_1, lesExpo1_2, lesExpo1_3,
                lesExpo2_1, lesExpo2_2,
                lesRN1_1, lesRN1_2,
                lesRN2_1, lesRN2_2,
                lesAWS1_1, lesAWS1_2,
                lesAWS2_1, lesAWS2_2
            );

            // ── Exercícios ──────────────────────────────────────────

            // ─ Lição: Introdução ao Expo ─
            var ex1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "O que é o Expo?", CorrectAnswer = "Um framework open-source para criar apps React Native" };
            var ex1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um framework open-source para criar apps React Native", IsCorrect = true };
            var ex1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Uma linguagem de programação para iOS", IsCorrect = false };
            var ex1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um banco de dados NoSQL mobile", IsCorrect = false };
            var ex1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um servidor de hospedagem para apps", IsCorrect = false };

            var ex2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "Qual comando cria um novo projeto Expo?", CorrectAnswer = "npx create-expo-app" };
            var ex2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "npx create-expo-app", IsCorrect = true };
            var ex2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "npm init expo", IsCorrect = false };
            var ex2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "expo start new", IsCorrect = false };
            var ex2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "yarn create expo-project", IsCorrect = false };

            var ex3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "O Expo permite desenvolvimento para Android e iOS com o mesmo código?", CorrectAnswer = "Verdadeiro" };
            var ex3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex3.Id, Text = "Verdadeiro", IsCorrect = true };
            var ex3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex3.Id, Text = "Falso", IsCorrect = false };

            // ─ Lição: Componentes Básicos ─
            var ex4 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_2.Id, Question = "Qual componente React Native é usado para exibir texto?", CorrectAnswer = "Text" };
            var ex4o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Text", IsCorrect = true };
            var ex4o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Label", IsCorrect = false };
            var ex4o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Paragraph", IsCorrect = false };
            var ex4o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Span", IsCorrect = false };

            var ex5 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_2.Id, Question = "No React Native, a `div` do HTML é substituída por:", CorrectAnswer = "View" };
            var ex5o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "View", IsCorrect = true };
            var ex5o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Container", IsCorrect = false };
            var ex5o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Box", IsCorrect = false };
            var ex5o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Section", IsCorrect = false };

            var ex5b = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_2.Id, Question = "Qual componente permite criar listas roláveis no React Native?", CorrectAnswer = "FlatList" };
            var ex5bo1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5b.Id, Text = "FlatList", IsCorrect = true };
            var ex5bo2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5b.Id, Text = "ListView", IsCorrect = false };
            var ex5bo3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5b.Id, Text = "ScrollList", IsCorrect = false };
            var ex5bo4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5b.Id, Text = "TableView", IsCorrect = false };

            // ─ Lição: StyleSheet e Flexbox ─
            var ex6 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_3.Id, Question = "Como se cria um StyleSheet no React Native?", CorrectAnswer = "StyleSheet.create({ ... })" };
            var ex6o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "StyleSheet.create({ ... })", IsCorrect = true };
            var ex6o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "new StyleSheet({ ... })", IsCorrect = false };
            var ex6o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "CSS.create({ ... })", IsCorrect = false };
            var ex6o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "createStyle({ ... })", IsCorrect = false };

            var ex6b = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_3.Id, Question = "Qual é a direção padrão do Flexbox no React Native?", CorrectAnswer = "column (vertical)" };
            var ex6bo1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6b.Id, Text = "column (vertical)", IsCorrect = true };
            var ex6bo2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6b.Id, Text = "row (horizontal)", IsCorrect = false };
            var ex6bo3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6b.Id, Text = "grid", IsCorrect = false };
            var ex6bo4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6b.Id, Text = "block", IsCorrect = false };

            var ex6c = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_3.Id, Question = "Para centralizar elementos horizontalmente com flexbox, usa-se:", CorrectAnswer = "alignItems: 'center'" };
            var ex6co1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6c.Id, Text = "alignItems: 'center'", IsCorrect = true };
            var ex6co2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6c.Id, Text = "justifyContent: 'center'", IsCorrect = false };
            var ex6co3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6c.Id, Text = "textAlign: 'center'", IsCorrect = false };
            var ex6co4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6c.Id, Text = "flex: 'center'", IsCorrect = false };

            // ─ Lição: Expo Router ─
            var ex_er1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_1.Id, Question = "O Expo Router usa roteamento baseado em:", CorrectAnswer = "Arquivos e pastas" };
            var ex_er1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er1.Id, Text = "Arquivos e pastas", IsCorrect = true };
            var ex_er1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er1.Id, Text = "Configuração JSON centralizada", IsCorrect = false };
            var ex_er1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er1.Id, Text = "Hooks de navegação manuais", IsCorrect = false };
            var ex_er1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er1.Id, Text = "Contextos globais", IsCorrect = false };

            var ex_er2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_1.Id, Question = "Qual pasta é a raiz das rotas no Expo Router?", CorrectAnswer = "app/" };
            var ex_er2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er2.Id, Text = "app/", IsCorrect = true };
            var ex_er2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er2.Id, Text = "pages/", IsCorrect = false };
            var ex_er2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er2.Id, Text = "screens/", IsCorrect = false };
            var ex_er2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er2.Id, Text = "routes/", IsCorrect = false };

            var ex_er3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_1.Id, Question = "Para navegar programaticamente no Expo Router, usa-se:", CorrectAnswer = "useRouter().push()" };
            var ex_er3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er3.Id, Text = "useRouter().push()", IsCorrect = true };
            var ex_er3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er3.Id, Text = "navigate.go()", IsCorrect = false };
            var ex_er3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er3.Id, Text = "history.push()", IsCorrect = false };
            var ex_er3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_er3.Id, Text = "Route.navigate()", IsCorrect = false };

            // ─ Lição: Stack e Tab Navigation ─
            var ex_st1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_2.Id, Question = "Qual componente cria navegação em pilha (Stack) no Expo Router?", CorrectAnswer = "Stack" };
            var ex_st1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st1.Id, Text = "Stack", IsCorrect = true };
            var ex_st1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st1.Id, Text = "Navigator", IsCorrect = false };
            var ex_st1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st1.Id, Text = "Pile", IsCorrect = false };
            var ex_st1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st1.Id, Text = "Screen", IsCorrect = false };

            var ex_st2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_2.Id, Question = "No Expo Router, Tabs criam navegação:", CorrectAnswer = "Por abas na parte inferior da tela" };
            var ex_st2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st2.Id, Text = "Por abas na parte inferior da tela", IsCorrect = true };
            var ex_st2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st2.Id, Text = "Por gaveta lateral deslizante", IsCorrect = false };
            var ex_st2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st2.Id, Text = "Por pilha de telas sobrepostas", IsCorrect = false };
            var ex_st2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st2.Id, Text = "Por modal popup", IsCorrect = false };

            var ex_st3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo2_2.Id, Question = "O arquivo _layout.tsx em Expo Router serve para:", CorrectAnswer = "Definir o layout compartilhado entre rotas" };
            var ex_st3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st3.Id, Text = "Definir o layout compartilhado entre rotas", IsCorrect = true };
            var ex_st3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st3.Id, Text = "Configurar variáveis de ambiente", IsCorrect = false };
            var ex_st3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st3.Id, Text = "Criar testes unitários", IsCorrect = false };
            var ex_st3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_st3.Id, Text = "Definir estilos globais do app", IsCorrect = false };

            // ─ Lição: useCallback e useMemo ─
            var ex7 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_1.Id, Question = "O hook `useCallback` serve para memoizar:", CorrectAnswer = "Funções" };
            var ex7o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Funções", IsCorrect = true };
            var ex7o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Componentes inteiros", IsCorrect = false };
            var ex7o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Estados", IsCorrect = false };
            var ex7o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Contextos globais", IsCorrect = false };

            var ex8 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_1.Id, Question = "O `useMemo` recalcula o valor somente quando:", CorrectAnswer = "As dependências informadas mudam" };
            var ex8o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "As dependências informadas mudam", IsCorrect = true };
            var ex8o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "O componente renderiza sempre", IsCorrect = false };
            var ex8o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "Nunca — é calculado uma única vez", IsCorrect = false };
            var ex8o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "Quando o estado global muda", IsCorrect = false };

            var ex8b = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_1.Id, Question = "Por que `useCallback` ajuda a evitar re-renders em componentes filhos?", CorrectAnswer = "Mantém a mesma referência da função entre renders" };
            var ex8bo1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8b.Id, Text = "Mantém a mesma referência da função entre renders", IsCorrect = true };
            var ex8bo2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8b.Id, Text = "Bloqueia o ciclo de renderização", IsCorrect = false };
            var ex8bo3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8b.Id, Text = "Executa a função fora do componente", IsCorrect = false };
            var ex8bo4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8b.Id, Text = "Remove a função do Virtual DOM", IsCorrect = false };

            // ─ Lição: React.memo ─
            var ex_rm1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_2.Id, Question = "React.memo evita re-renders quando:", CorrectAnswer = "As props do componente não mudaram" };
            var ex_rm1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm1.Id, Text = "As props do componente não mudaram", IsCorrect = true };
            var ex_rm1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm1.Id, Text = "O estado global mudou", IsCorrect = false };
            var ex_rm1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm1.Id, Text = "O contexto do app mudou", IsCorrect = false };
            var ex_rm1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm1.Id, Text = "O componente é um componente de classe", IsCorrect = false };

            var ex_rm2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_2.Id, Question = "React.memo é um:", CorrectAnswer = "Higher-Order Component (HOC)" };
            var ex_rm2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm2.Id, Text = "Higher-Order Component (HOC)", IsCorrect = true };
            var ex_rm2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm2.Id, Text = "Hook do React", IsCorrect = false };
            var ex_rm2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm2.Id, Text = "Método de ciclo de vida", IsCorrect = false };
            var ex_rm2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm2.Id, Text = "Context Provider", IsCorrect = false };

            var ex_rm3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_2.Id, Question = "React.memo compara props usando:", CorrectAnswer = "Comparação rasa (shallow comparison)" };
            var ex_rm3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm3.Id, Text = "Comparação rasa (shallow comparison)", IsCorrect = true };
            var ex_rm3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm3.Id, Text = "Comparação profunda (deep comparison)", IsCorrect = false };
            var ex_rm3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm3.Id, Text = "Serialização JSON", IsCorrect = false };
            var ex_rm3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_rm3.Id, Text = "Hash MD5 das props", IsCorrect = false };

            // ─ Lição: Context API ─
            var ex_ctx1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_1.Id, Question = "O hook para consumir um Context no React é:", CorrectAnswer = "useContext" };
            var ex_ctx1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx1.Id, Text = "useContext", IsCorrect = true };
            var ex_ctx1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx1.Id, Text = "useConsume", IsCorrect = false };
            var ex_ctx1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx1.Id, Text = "useProvider", IsCorrect = false };
            var ex_ctx1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx1.Id, Text = "useStore", IsCorrect = false };

            var ex_ctx2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_1.Id, Question = "Para criar um Context no React, usamos:", CorrectAnswer = "React.createContext()" };
            var ex_ctx2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx2.Id, Text = "React.createContext()", IsCorrect = true };
            var ex_ctx2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx2.Id, Text = "new Context()", IsCorrect = false };
            var ex_ctx2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx2.Id, Text = "useState()", IsCorrect = false };
            var ex_ctx2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx2.Id, Text = "Context.create()", IsCorrect = false };

            var ex_ctx3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_1.Id, Question = "O componente Provider no Context API serve para:", CorrectAnswer = "Disponibilizar o valor do contexto para todos os filhos" };
            var ex_ctx3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx3.Id, Text = "Disponibilizar o valor do contexto para todos os filhos", IsCorrect = true };
            var ex_ctx3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx3.Id, Text = "Consumir dados de uma API REST", IsCorrect = false };
            var ex_ctx3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx3.Id, Text = "Criar rotas de navegação", IsCorrect = false };
            var ex_ctx3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_ctx3.Id, Text = "Gerenciar animações", IsCorrect = false };

            // ─ Lição: Zustand e Redux Toolkit ─
            var ex_zrd1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_2.Id, Question = "O Zustand é uma biblioteca de:", CorrectAnswer = "Gerenciamento de estado global minimalista" };
            var ex_zrd1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd1.Id, Text = "Gerenciamento de estado global minimalista", IsCorrect = true };
            var ex_zrd1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd1.Id, Text = "Roteamento de telas", IsCorrect = false };
            var ex_zrd1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd1.Id, Text = "Estilização de componentes", IsCorrect = false };
            var ex_zrd1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd1.Id, Text = "Consumo de APIs REST", IsCorrect = false };

            var ex_zrd2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_2.Id, Question = "No Redux Toolkit, uma 'slice' define:", CorrectAnswer = "Um pedaço do estado global com reducers e actions" };
            var ex_zrd2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd2.Id, Text = "Um pedaço do estado global com reducers e actions", IsCorrect = true };
            var ex_zrd2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd2.Id, Text = "Um componente de interface gráfica", IsCorrect = false };
            var ex_zrd2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd2.Id, Text = "Uma rota de navegação", IsCorrect = false };
            var ex_zrd2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd2.Id, Text = "Uma configuração de rede", IsCorrect = false };

            var ex_zrd3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN2_2.Id, Question = "Zustand usa um hook personalizado para acessar o estado. Esse hook é criado com:", CorrectAnswer = "create()" };
            var ex_zrd3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd3.Id, Text = "create()", IsCorrect = true };
            var ex_zrd3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd3.Id, Text = "useState()", IsCorrect = false };
            var ex_zrd3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd3.Id, Text = "createSlice()", IsCorrect = false };
            var ex_zrd3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_zrd3.Id, Text = "useStore()", IsCorrect = false };

            // ─ Lição: IAM e Segurança ─
            var ex9 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_1.Id, Question = "IAM na AWS significa:", CorrectAnswer = "Identity and Access Management" };
            var ex9o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Identity and Access Management", IsCorrect = true };
            var ex9o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Internet Application Module", IsCorrect = false };
            var ex9o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Integrated Auth Middleware", IsCorrect = false };
            var ex9o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Internal Access Manager", IsCorrect = false };

            var ex10 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_1.Id, Question = "No IAM, o que é uma Policy?", CorrectAnswer = "Um documento JSON que define permissões" };
            var ex10o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um documento JSON que define permissões", IsCorrect = true };
            var ex10o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um servidor de autenticação", IsCorrect = false };
            var ex10o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um tipo de usuário administrador", IsCorrect = false };
            var ex10o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Uma região geográfica da AWS", IsCorrect = false };

            var ex10b = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_1.Id, Question = "O Princípio do Menor Privilégio no IAM significa:", CorrectAnswer = "Dar apenas as permissões necessárias para cada usuário" };
            var ex10bo1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10b.Id, Text = "Dar apenas as permissões necessárias para cada usuário", IsCorrect = true };
            var ex10bo2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10b.Id, Text = "Dar acesso total a todos os usuários", IsCorrect = false };
            var ex10bo3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10b.Id, Text = "Bloquear todos os usuários por padrão", IsCorrect = false };
            var ex10bo4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10b.Id, Text = "Compartilhar credenciais entre equipes", IsCorrect = false };

            // ─ Lição: Amazon S3 Básico ─
            var ex_s3_1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_2.Id, Question = "S3 na AWS significa:", CorrectAnswer = "Simple Storage Service" };
            var ex_s3_1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_1.Id, Text = "Simple Storage Service", IsCorrect = true };
            var ex_s3_1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_1.Id, Text = "Secure Server System", IsCorrect = false };
            var ex_s3_1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_1.Id, Text = "Scalable Static Service", IsCorrect = false };
            var ex_s3_1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_1.Id, Text = "Swift Storage Solution", IsCorrect = false };

            var ex_s3_2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_2.Id, Question = "Um bucket S3 contém:", CorrectAnswer = "Objetos (arquivos e metadados)" };
            var ex_s3_2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_2.Id, Text = "Objetos (arquivos e metadados)", IsCorrect = true };
            var ex_s3_2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_2.Id, Text = "Tabelas relacionais", IsCorrect = false };
            var ex_s3_2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_2.Id, Text = "Funções Lambda", IsCorrect = false };
            var ex_s3_2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_2.Id, Text = "Usuários IAM", IsCorrect = false };

            var ex_s3_3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_2.Id, Question = "O S3 cobra com base em:", CorrectAnswer = "Armazenamento usado e transferência de dados" };
            var ex_s3_3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_3.Id, Text = "Armazenamento usado e transferência de dados", IsCorrect = true };
            var ex_s3_3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_3.Id, Text = "Número de usuários cadastrados", IsCorrect = false };
            var ex_s3_3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_3.Id, Text = "Tempo de CPU utilizado", IsCorrect = false };
            var ex_s3_3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_s3_3.Id, Text = "Quantidade de requests por minuto", IsCorrect = false };

            // ─ Lição: Configurando Cognito ─
            var ex_cog1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_1.Id, Question = "Amazon Cognito é um serviço de:", CorrectAnswer = "Autenticação e gerenciamento de usuários" };
            var ex_cog1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog1.Id, Text = "Autenticação e gerenciamento de usuários", IsCorrect = true };
            var ex_cog1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog1.Id, Text = "Banco de dados NoSQL", IsCorrect = false };
            var ex_cog1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog1.Id, Text = "Hospedagem de sites estáticos", IsCorrect = false };
            var ex_cog1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog1.Id, Text = "Processamento de pagamentos", IsCorrect = false };

            var ex_cog2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_1.Id, Question = "No Cognito, um 'User Pool' é:", CorrectAnswer = "Um diretório de usuários com suporte a login/senha" };
            var ex_cog2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog2.Id, Text = "Um diretório de usuários com suporte a login/senha", IsCorrect = true };
            var ex_cog2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog2.Id, Text = "Um servidor de APIs", IsCorrect = false };
            var ex_cog2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog2.Id, Text = "Um grupo de permissões IAM", IsCorrect = false };
            var ex_cog2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog2.Id, Text = "Uma região AWS dedicada", IsCorrect = false };

            var ex_cog3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_1.Id, Question = "O Cognito pode ser integrado com provedores de identidade externos como:", CorrectAnswer = "Google, Facebook e Apple" };
            var ex_cog3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog3.Id, Text = "Google, Facebook e Apple", IsCorrect = true };
            var ex_cog3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog3.Id, Text = "Apenas usuários internos da AWS", IsCorrect = false };
            var ex_cog3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog3.Id, Text = "Somente Active Directory", IsCorrect = false };
            var ex_cog3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_cog3.Id, Text = "Nenhum provedor externo", IsCorrect = false };

            // ─ Lição: Tokens JWT no Cognito ─
            var ex_jwt1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_2.Id, Question = "JWT significa:", CorrectAnswer = "JSON Web Token" };
            var ex_jwt1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt1.Id, Text = "JSON Web Token", IsCorrect = true };
            var ex_jwt1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt1.Id, Text = "Java Web Template", IsCorrect = false };
            var ex_jwt1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt1.Id, Text = "JavaScript Wrapper Type", IsCorrect = false };
            var ex_jwt1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt1.Id, Text = "JSON Worker Thread", IsCorrect = false };

            var ex_jwt2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_2.Id, Question = "Um JWT é composto por quantas partes separadas por ponto?", CorrectAnswer = "3 partes: Header, Payload e Signature" };
            var ex_jwt2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt2.Id, Text = "3 partes: Header, Payload e Signature", IsCorrect = true };
            var ex_jwt2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt2.Id, Text = "2 partes: Token e Signature", IsCorrect = false };
            var ex_jwt2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt2.Id, Text = "4 partes: Header, Body, Payload e Hash", IsCorrect = false };
            var ex_jwt2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt2.Id, Text = "1 parte única codificada", IsCorrect = false };

            var ex_jwt3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS2_2.Id, Question = "O Refresh Token do Cognito serve para:", CorrectAnswer = "Obter novos Access Tokens sem novo login" };
            var ex_jwt3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt3.Id, Text = "Obter novos Access Tokens sem novo login", IsCorrect = true };
            var ex_jwt3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt3.Id, Text = "Fazer login no console AWS", IsCorrect = false };
            var ex_jwt3o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt3.Id, Text = "Deletar a conta do usuário", IsCorrect = false };
            var ex_jwt3o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex_jwt3.Id, Text = "Criptografar os dados do S3", IsCorrect = false };

            context.Exercises.AddRange(
                ex1, ex2, ex3,
                ex4, ex5, ex5b,
                ex6, ex6b, ex6c,
                ex_er1, ex_er2, ex_er3,
                ex_st1, ex_st2, ex_st3,
                ex7, ex8, ex8b,
                ex_rm1, ex_rm2, ex_rm3,
                ex_ctx1, ex_ctx2, ex_ctx3,
                ex_zrd1, ex_zrd2, ex_zrd3,
                ex9, ex10, ex10b,
                ex_s3_1, ex_s3_2, ex_s3_3,
                ex_cog1, ex_cog2, ex_cog3,
                ex_jwt1, ex_jwt2, ex_jwt3
            );

            context.ExerciseOptions.AddRange(
                ex1o1, ex1o2, ex1o3, ex1o4,
                ex2o1, ex2o2, ex2o3, ex2o4,
                ex3o1, ex3o2,
                ex4o1, ex4o2, ex4o3, ex4o4,
                ex5o1, ex5o2, ex5o3, ex5o4,
                ex5bo1, ex5bo2, ex5bo3, ex5bo4,
                ex6o1, ex6o2, ex6o3, ex6o4,
                ex6bo1, ex6bo2, ex6bo3, ex6bo4,
                ex6co1, ex6co2, ex6co3, ex6co4,
                ex_er1o1, ex_er1o2, ex_er1o3, ex_er1o4,
                ex_er2o1, ex_er2o2, ex_er2o3, ex_er2o4,
                ex_er3o1, ex_er3o2, ex_er3o3, ex_er3o4,
                ex_st1o1, ex_st1o2, ex_st1o3, ex_st1o4,
                ex_st2o1, ex_st2o2, ex_st2o3, ex_st2o4,
                ex_st3o1, ex_st3o2, ex_st3o3, ex_st3o4,
                ex7o1, ex7o2, ex7o3, ex7o4,
                ex8o1, ex8o2, ex8o3, ex8o4,
                ex8bo1, ex8bo2, ex8bo3, ex8bo4,
                ex_rm1o1, ex_rm1o2, ex_rm1o3, ex_rm1o4,
                ex_rm2o1, ex_rm2o2, ex_rm2o3, ex_rm2o4,
                ex_rm3o1, ex_rm3o2, ex_rm3o3, ex_rm3o4,
                ex_ctx1o1, ex_ctx1o2, ex_ctx1o3, ex_ctx1o4,
                ex_ctx2o1, ex_ctx2o2, ex_ctx2o3, ex_ctx2o4,
                ex_ctx3o1, ex_ctx3o2, ex_ctx3o3, ex_ctx3o4,
                ex_zrd1o1, ex_zrd1o2, ex_zrd1o3, ex_zrd1o4,
                ex_zrd2o1, ex_zrd2o2, ex_zrd2o3, ex_zrd2o4,
                ex_zrd3o1, ex_zrd3o2, ex_zrd3o3, ex_zrd3o4,
                ex9o1, ex9o2, ex9o3, ex9o4,
                ex10o1, ex10o2, ex10o3, ex10o4,
                ex10bo1, ex10bo2, ex10bo3, ex10bo4,
                ex_s3_1o1, ex_s3_1o2, ex_s3_1o3, ex_s3_1o4,
                ex_s3_2o1, ex_s3_2o2, ex_s3_2o3, ex_s3_2o4,
                ex_s3_3o1, ex_s3_3o2, ex_s3_3o3, ex_s3_3o4,
                ex_cog1o1, ex_cog1o2, ex_cog1o3, ex_cog1o4,
                ex_cog2o1, ex_cog2o2, ex_cog2o3, ex_cog2o4,
                ex_cog3o1, ex_cog3o2, ex_cog3o3, ex_cog3o4,
                ex_jwt1o1, ex_jwt1o2, ex_jwt1o3, ex_jwt1o4,
                ex_jwt2o1, ex_jwt2o2, ex_jwt2o3, ex_jwt2o4,
                ex_jwt3o1, ex_jwt3o2, ex_jwt3o3, ex_jwt3o4
            );

            // ── Usuários seed para o ranking ───────────────────────
            var seedUsers = new[]
            {
                new User { Id = Guid.NewGuid(), Name = "Ana Beatriz",   Email = "ana@seed.dev",    PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 920, Level = 9,  Streak = 21 },
                new User { Id = Guid.NewGuid(), Name = "Carlos Lima",   Email = "carlos@seed.dev", PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 750, Level = 7,  Streak = 14 },
                new User { Id = Guid.NewGuid(), Name = "Beatriz Melo",  Email = "beatriz@seed.dev",PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 580, Level = 5,  Streak = 9  },
                new User { Id = Guid.NewGuid(), Name = "Diego Souza",   Email = "diego@seed.dev",  PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 420, Level = 4,  Streak = 7  },
                new User { Id = Guid.NewGuid(), Name = "Larissa Kato",  Email = "larissa@seed.dev",PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 310, Level = 3,  Streak = 5  },
                new User { Id = Guid.NewGuid(), Name = "Rafael Neto",   Email = "rafael@seed.dev", PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 220, Level = 2,  Streak = 3  },
                new User { Id = Guid.NewGuid(), Name = "Mariana Torres",Email = "mariana@seed.dev",PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 140, Level = 1,  Streak = 2  },
                new User { Id = Guid.NewGuid(), Name = "Pedro Alves",   Email = "pedro@seed.dev",  PasswordHash = PasswordHasher.HashPassword("seed123"), XP = 80,  Level = 0,  Streak = 1  },
            };
            context.Users.AddRange(seedUsers);

            await context.SaveChangesAsync();
        }
    }
}
